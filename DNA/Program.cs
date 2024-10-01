#region Usings

using Azure.Identity;
using DNA3.Classes;
using DNA3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using DocumentFormat.OpenXml.InkML;

#endregion

#region Configuration

var builder = WebApplication.CreateBuilder(args);

// Add external configuration file
builder.Configuration.AddJsonFile("C:\\DNASettings.json");

// Make sure the JWT Key is accessible to the Configuration Manager
string? jwtkey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtkey)) {
    throw new ArgumentException("JWT Encryption Key must be configured and accessible to the Configuration Manager");
}

// Make sure the Connection String is accessible to the Configuration Manager
string? connectionstring = builder.Configuration["ConnectionStrings:MainContext"];
if (string.IsNullOrEmpty(connectionstring)) {
    throw new ArgumentException("Database Connection String must be configured and accessible to the Configuration Manager");
} else {
    Utilities.Site.ConnectionString = connectionstring;
}

// Make sure the Google Client ID and Secret are accessible to the Configuration Manager
string? googleClientId = builder.Configuration["GoogleSettings:ClientId"];
if (string.IsNullOrEmpty(googleClientId)) {
    throw new ArgumentException("Google Client ID must be configured and accessible to the Configuration Manager");
}
string? googleClientSecret = builder.Configuration["GoogleSettings:ClientSecret"];
if (string.IsNullOrEmpty(googleClientSecret)) {
    throw new ArgumentException("Google Client Secret must be configured and accessible to the Configuration Manager");
}

// Services 
var services = builder.Services;
services.AddControllersWithViews();
services.AddMvc();

// Dashboard Assembly Part
Assembly DashboardAssembly = typeof(DNA3.Controllers.DashboardController).GetTypeInfo().Assembly;
AssemblyPart DashboardPart = new(DashboardAssembly);
services.AddControllersWithViews().ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(DashboardPart));

// Authentication Options (Change CookieAuthenticatonDefaults to CertificateAuthenticationDefaults for User Mapped Client Certificate Authentication)
services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddCookie(options => {
    options.LoginPath = "/Dashboard/Login";
    options.LogoutPath = "/Dashboard/Logout";
    options.AccessDeniedPath = "/Dashboard";
    options.SlidingExpiration = true;
}).AddCertificate(options => {
    options.AllowedCertificateTypes = CertificateTypes.All;
    options.Events = new CertificateAuthenticationEvents {
        OnCertificateValidated = async context => {
            IAuth validationService = context.HttpContext.RequestServices.GetRequiredService<IAuth>();
            await validationService.AuthenticateCertificate(context);
        }
    };
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        RequireExpirationTime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtkey))
    };
}).AddGoogle(googleOptions => {
    googleOptions.ClientId = googleClientId;
    googleOptions.ClientSecret = googleClientSecret;
    googleOptions.Scope.Add("email");
    googleOptions.Scope.Add("profile");
    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "EmailAddress");
    googleOptions.Events.OnTicketReceived += Handlers.GoogleOnTicketReceived;
});

// Authorization Policies
services.AddAuthorizationBuilder()
    .AddPolicy("ApiUserPolicy", policy => policy.RequireClaim("Role", "Admin"))
    .AddPolicy("Administrators", policy => policy.RequireClaim("Role", "Admin"))
    .AddPolicy("Managers", policy => policy.RequireClaim("Role", "Admin", "Manager"))
    .AddPolicy("Users", policy => policy.RequireClaim("Role", "Admin", "Manager", "User"));

// Cookie Policy
services.Configure<CookiePolicyOptions>(options => {
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// Database Context
services.AddDbContext<MainContext>(options => options.UseSqlServer(connectionstring));

// Cross-Origin Requests
services.AddCors();

// Session Management
services.AddDistributedMemoryCache();
services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Data Protection
services.AddDataProtection();

// Other Services
services.AddScoped<DNA3.Classes.IAuth, DNA3.Classes.Auth>();
services.AddScoped<DNA3.Classes.ITools, DNA3.Classes.Tools>();
services.AddScoped<Utilities.IHttp, Utilities.Http>();
services.AddScoped<IDNATools, DNATools>();
services.AddHttpContextAccessor();

// Repect Browser Accespt Header and Localization, add XML Serializer Formatters
services.AddMvc(options => {
    options.RespectBrowserAcceptHeader = true;
})
  .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
  .AddDataAnnotationsLocalization()
  .AddXmlSerializerFormatters();

// Controller Options
services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true);

// Kestrel Server Options (Uncomment for User Mapped Client Certificate Authentication)
//services.Configure<KestrelServerOptions>(options => {
//    options.ConfigureHttpsDefaults(options => options.ClientCertificateMode = ClientCertificateMode.RequireCertificate);
//});

var app = builder.Build();

// Error Handling
//app.UseDeveloperExceptionPage();
app.UseExceptionHandler("/Home/Error");
app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

// Global Cross-Origin Request Policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Forwarded Headers
app.UseForwardedHeaders(new ForwardedHeadersOptions {
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Services
app.UseHttpsRedirection();
app.UseStaticFiles();
//if (Directory.Exists(@"C:\Virtual\Homeowners")) {
//    app.UseStaticFiles(new StaticFileOptions {
//        FileProvider = new PhysicalFileProvider(@"C:\Virtual\Homeowners"),
//        RequestPath = "/homeowners"
//    });
//}
app.UseCookiePolicy();
app.UseRouting();

// Session (Muat be called after UseRouting and before UseEndpoints)
app.UseSession();

// Security (Must be located between Routing and Endpoints)
app.UseAuthentication();
app.UseAuthorization();

// Routing
app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists=Dashboard}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Syncfusion
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["Syncfusion:License"]);
Bold.Licensing.BoldLicenseProvider.RegisterLicense(builder.Configuration["Boldreports:License"]);

app.Run();

#endregion

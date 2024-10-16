#region Usings

using DNA3.Classes;
using DNA3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;

#endregion

#region Configuration Manager Sources

var builder = WebApplication.CreateBuilder(args);

// The Configuration Manager can load settings from a wide variety of sources including JSON Files, XML Files, INI Files, Command-Line Arguments,
// Environment Variables, In-Memory .NET Objects, Secret Manager Storage, and the Azure Key Vault. In this case, we load application secrets from
// an encrypted JSON file stored outside of the source-code tree to prevent propagation to Version Control archives.
builder.Configuration.AddJsonFile("C:\\DNASettings.json");

// JWT Key must be accessible to the Configuration Manager
string? jwtkey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtkey)) {
    throw new ArgumentException("JWT Encryption Key must be configured and accessible to the Configuration Manager");
}

// Connection String must be accessible to the Configuration Manager
string? connectionstring = builder.Configuration["ConnectionStrings:MainContext"];
if (string.IsNullOrEmpty(connectionstring)) {
    throw new ArgumentException("Database Connection String must be configured and accessible to the Configuration Manager");
} else {
    Utilities.Site.ConnectionString = connectionstring;
}

// Google Client ID and Secret must be accessible to the Configuration Manager
string? googleClientId = builder.Configuration["GoogleSettings:ClientId"];
if (string.IsNullOrEmpty(googleClientId)) {
    throw new ArgumentException("Google Client ID must be configured and accessible to the Configuration Manager");
}
string? googleClientSecret = builder.Configuration["GoogleSettings:ClientSecret"];
if (string.IsNullOrEmpty(googleClientSecret)) {
    throw new ArgumentException("Google Client Secret must be configured and accessible to the Configuration Manager");
}

// Common application settings must be accessible to the Configuration Manager
string? appname = builder.Configuration["App:Name"];
if (string.IsNullOrEmpty(appname)) {
    throw new ArgumentException("Application Name must be configured and accessible to the Configuration Manager");
}
string? appdescription = builder.Configuration["App:Description"];
if (string.IsNullOrEmpty(appdescription)) {
    throw new ArgumentException("Application Description must be configured and accessible to the Configuration Manager");
}
string? appversion = builder.Configuration["App:Version"];
if (string.IsNullOrEmpty(appversion)) {
    throw new ArgumentException("Application Version must be configured and accessible to the Configuration Manager");
}
string? appurl = builder.Configuration["App:URL"];
if (string.IsNullOrEmpty(appurl)) {
    throw new ArgumentException("Application URL must be configured and accessible to the Configuration Manager");
}

#endregion

// Services 
var services = builder.Services;
services.AddControllersWithViews();
services.AddMvc();

// Dashboard Assembly Part
Assembly DashboardAssembly = typeof(DNA3.Controllers.DashboardController).GetTypeInfo().Assembly;
AssemblyPart DashboardPart = new(DashboardAssembly);
services.AddControllersWithViews().ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(DashboardPart));

// Cookie Authentication
//
//services.AddAuthentication(options => {
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//
// Certificate Authentication (Also uncomment Kestrel options below)
//
//services.AddAuthentication(options => {
//    options.DefaultAuthenticateScheme = CertificateAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CertificateAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = CertificateAuthenticationDefaults.AuthenticationScheme;
//
// Google Authentication
//
//services.AddAuthentication(options => {
//    options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//
services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
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

// Encrypted Data Protection
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

// Swagger Generator and Documents
services.AddSwaggerGen(c => {
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); // Required for Telerik Reporting
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);
    c.SwaggerDoc(appversion, new OpenApiInfo {
        Version = appversion,
        Title = appname,
        Description = appdescription,
        TermsOfService = new Uri($"{appurl}/Terms"),
        Contact = new OpenApiContact() { Name = builder.Configuration["App:SupportName"], Email = builder.Configuration["App:SupportAddress"], Url = new Uri(appurl) }
    });
});


// Kestrel Server Options (Uncomment for User Mapped Client Certificate Authentication during development)
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

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint($"/swagger/{appversion}/swagger.json", appname);
});

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

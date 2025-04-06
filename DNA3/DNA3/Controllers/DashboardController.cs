#region Usings

using DNA3.Classes;
using DNA3.Models;
using Google.Authenticator;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using Serilog;
using System;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

    [Authorize]
    //[Authorize(AuthenticationSchemes = CertificateAuthenticationDefaults.AuthenticationScheme)]
    public class DashboardController : Controller {

        #region Variables

        // Services
        private readonly IConfiguration Configuration;
        private readonly MainContext Context;
        private readonly ILogger<DashboardController> Logger;
        private readonly IAuth Auth;
        private readonly ITools Tools;
        private readonly IDNATools DNATools;

        #endregion

        #region Class Methods and Events

        public DashboardController(IConfiguration configuration, MainContext context, ILogger<DashboardController> logger, IAuth auth, ITools tools, IDNATools dnatools) {
            Configuration = configuration;
            Context = context;
            Logger = logger;
            Auth = auth;
            Tools = tools;
            DNATools = dnatools;
        }

        #endregion

        #region Controller Actions

        // Seed Database
        [HttpGet]
        [Authorize(Policy = "Administrators")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SeedAsync() {
            try {
                await DNATools.Initialize();
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return RedirectToAction("Index");
        }

        // Initialize Database
        [HttpGet]
        [Authorize(Policy = "Administrators")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Initialize() {
            Initialize instance = default;
            try {
                instance = new Initialize {
                    Account = false,
                    Activity = false,
                    Application = false,
                    Article = false,
                    Asset = false,
                    Associate = false,
                    Campaign = false,
                    Cart = false,
                    Category = false,
                    Certificate = false,
                    Client = false,
                    Commission = false,
                    Customer = false,
                    CustomerComment = false,
                    Device = false,
                    Disposition = false,
                    Documents = false,
                    Facility = false,
                    History = false,
                    Homeowner = false,
                    Invoice = false,
                    InvoiceItem = false,
                    Item = false,
                    Material = false,
                    Menu = false,
                    Note = false,
                    Page = false,
                    Password = false,
                    Product = false,
                    Promotion = false,
                    Reference = false,
                    Registration = false,
                    Request = false,
                    Residence = false,
                    Resource = false,
                    Role = false,
                    Section = false,
                    Source = false,
                    Status = false,
                    Task = false,
                    Ticket = false,
                    User = false,
                    Vendor = false,
                    Workorder = false,
                    WorkorderComment = false,
                    Zone = false
                };
                Site.Messages.Enqueue("<strong>WARNING:</strong> This action cannot be undone!");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return View(instance);
        }

        // Initialize
        [HttpPost]
        [Authorize(Policy = "Administrators")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Initialize(Initialize instance) {
            int rows;
            try {
                using (SqlConnection conn = new(Configuration["ConnectionStrings:MainContext"])) {

                    conn.Open();

                    // Account
                    if (instance.Account) {
                        using SqlCommand cmd = new("TRUNCATE TABLE Account", conn); rows = cmd.ExecuteNonQuery();
                    }

                    // Activity
                    if (instance.Activity) {
                        using SqlCommand cmd = new("TRUNCATE TABLE Activity", conn); rows = cmd.ExecuteNonQuery();
                    }

                    // Application
                    if (instance.Application) {
                        using SqlCommand cmd = new("TRUNCATE TABLE Application", conn); rows = cmd.ExecuteNonQuery();
                    }

                    // Article
                    if (instance.Article) {
                        using SqlCommand cmd = new("TRUNCATE TABLE Article", conn); rows = cmd.ExecuteNonQuery();
                    }

                    // Asset
                    if (instance.Asset) {
                        using SqlCommand cmd = new("TRUNCATE TABLE Asset", conn); rows = cmd.ExecuteNonQuery();
                    }

                    // Associate
                    if (instance.Associate) {
                        using SqlCommand cmd = new("TRUNCATE TABLE Associate", conn); rows = cmd.ExecuteNonQuery();
                    }

                    // Campaign
                    if (instance.Campaign) {
                        using SqlCommand cmd = new("TRUNCATE TABLE Campaign", conn); rows = cmd.ExecuteNonQuery();
                    }

                    // Cart
                    if (instance.Cart) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Cart", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Category
                    if (instance.Category) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Category", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Certificate
                    if (instance.Certificate) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Certificate", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Client
                    if (instance.Client) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Client", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Commission
                    if (instance.Commission) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Commission", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Customer
                    if (instance.Customer) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Customer", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Customer Comment
                    if (instance.CustomerComment) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE CustomerComment", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Device
                    if (instance.Device) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Device", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Disposition
                    if (instance.Disposition) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Disposition", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Documents
                    if (instance.Documents) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Documents", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Facility
                    if (instance.Facility) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Facility", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // History
                    if (instance.History) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE History", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Home Owner
                    if (instance.Homeowner) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Homeowner", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Invoice
                    if (instance.Invoice) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Invoice", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Invoice Item
                    if (instance.InvoiceItem) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE InvoiceItem", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Item
                    if (instance.Item) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Item", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Material
                    if (instance.Material) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Material", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Menu
                    if (instance.Menu) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Action", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                        using (SqlCommand cmd = new("TRUNCATE TABLE Menu", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Note
                    if (instance.Note) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Note", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Page
                    if (instance.Page) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Page", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Password
                    if (instance.Password) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Password", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Product
                    if (instance.Product) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Product", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Promotion
                    if (instance.Promotion) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Promotion", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Request
                    if (instance.Request) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Request", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Residence
                    if (instance.Residence) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Residence", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Resource
                    if (instance.Resource) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Resource", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Role
                    if (instance.Role) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Role", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Section
                    if (instance.Section) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Section", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Source
                    if (instance.Source) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Source", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Status
                    if (instance.Status) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Status", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Task
                    if (instance.Task) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Task", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Ticket
                    if (instance.Ticket) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Ticket", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // User
                    if (instance.User) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE User", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Vendor
                    if (instance.Vendor) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Vendor", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Workorder
                    if (instance.Workorder) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Workorder", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Workorder Comment
                    if (instance.WorkorderComment) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE WorkorderComment", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    // Zone
                    if (instance.Zone) {
                        using (SqlCommand cmd = new("TRUNCATE TABLE Zone", conn)) {
                            rows = cmd.ExecuteNonQuery();
                        }
                    }

                    if (conn.State == System.Data.ConnectionState.Open) {
                        conn.Close();
                    }

                }

                Site.Messages.Enqueue("Process completed successfully");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return View(instance);
        }

        // Index
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index() {
            HomeView instance = new();
            try {
                if (Utilities.Ado.DatabaseExists(Configuration["ConnectionStrings:MainContext"])) {
                    if (!Utilities.Ado.DatabaseTableExists(Configuration["ConnectionStrings:MainContext"], "Login")) {
                        // Run Migration Script and Seed Database (Requires Database Owner Credentials)
                    }
                } else {
                    // Create Database, Run Migration Script, and Seed Database (Requires System Administrator Credentials)
                }
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return View(instance);
        }

        // Contact (Get)
        public IActionResult Contact(string type) {
            try {
                var request = new Request {
                    Date = DateTime.Now,
                    Type = type,
                    Subscribe = "True"
                };
                ViewData["BodyClass"] = "";
                return View(request);
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return View();
        }

        // Contact (Post)
        [HttpPost]
        public async Task<IActionResult> Contact([FromForm] Request request) {
            try {
                if (ModelState.IsValid) {
                    Context.Request.Add(request);
                    await Context.SaveChangesAsync();

                    // Create Message
                    MailMessage message = new() {
                        From = new MailAddress(Configuration["Smtp:From"], Configuration["App:SalesName"]),
                        Subject = $"[{Request.HttpContext.Connection.RemoteIpAddress}] - {request.Subject}",
                        Body = request.Content,
                        BodyEncoding = System.Text.Encoding.UTF8,
                        IsBodyHtml = true
                    };
                    message.To.Add(new MailAddress(Configuration["Smtp:From"], Configuration["App:SalesName"]));
                    message.CC.Add(new MailAddress(request.Email, $"{request.First} {request.Last}"));
                    message.ReplyToList.Add(new MailAddress(request.Email, $"{request.First} {request.Last}"));

                    NetworkCredential credentials = new(Configuration["Smtp:User"], Configuration["Smtp:Password"]);

                    System.Net.Mail.SmtpClient smtpclient = new() {
                        Host = Configuration["Smtp:Host"],
                        Port = Convert.ToInt16(Configuration["Smtp:Port"]),
                        Credentials = credentials,
                        EnableSsl = true
                    };

                    smtpclient.Send(message);

                    Site.Messages.Enqueue("Your request was sent successfully. Check your e-mail we usually respond within minutes!");
                    return RedirectToAction("Index");
                } else {
                    Site.Messages.Enqueue("Please correct the errors indicated below and try again.");
                };
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return View("Contact", request);
        }

        // Enable (MFA)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Enable(User user) {
            String message = "";
            try {
                if (ModelState.IsValid) {
                    if (user.TotpCode != null) {
                        TwoFactorAuthenticator tfa = new();
                        if (tfa.ValidateTwoFactorPIN(user.TotpKey, user.TotpCode) == true) {
                            user.TotpCode = "";
                            Context.User.Update(user);
                            await Context.SaveChangesAsync();
                            message = "MFA Enabled Successfully";
                            Site.Messages.Enqueue(message);
                            Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                        }
                    }
                }
                ViewBag.RoleList = await Context.Role.OrderBy(x => x.Description).ToListAsync();
                ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
                ViewData["Logins"] = await Context.Login.Where(x => x.UserId == User.UserId()).OrderBy(x => x.Email).ToListAsync();
                ViewData["Client"] = await Context.Client.FindAsync(user.ClientId);
                return View("Profile", user);
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }

        // Disable (MFA)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Disable(User user) {
            String message = "";
            try {
                if (ModelState.IsValid) {
                    if (user.TotpCode != null) {
                        TwoFactorAuthenticator tfa = new();
                        if (tfa.ValidateTwoFactorPIN("Move this to identity management", user.TotpCode) == true) {
                            user.TotpCode = "";
                            user.TotpKey = "";
                            Context.User.Update(user);
                            await Context.SaveChangesAsync();
                            message = "MFA Disabled Successfully";
                            Site.Messages.Enqueue(message);
                            Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                        }
                    }
                }
                ViewBag.RoleList = await Context.Role.OrderBy(x => x.Description).ToListAsync();
                ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
                ViewData["Logins"] = await Context.Login.Where(x => x.UserId == User.UserId()).OrderBy(x => x.Email).ToListAsync();
                ViewData["Client"] = await Context.Client.FindAsync(user.ClientId);
                return View("Profile", user);
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }

        // File Manager (Get)
        [HttpGet]
        [Authorize(Policy = "Managers")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult FileManager() {
            return View();
        }

        // Github (Get)
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Github() {
            return Challenge(new AuthenticationProperties { }, "GitHub");
        }

        // Login (Get)
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Login() {
            Claimant claimant = new();
            try {
                string url = HttpContext.Request.Query["ReturnUrl"].ToString();
                if (User.Identity.IsAuthenticated) {
                    if (!String.IsNullOrEmpty(url) && Url.IsLocalUrl(url)) {
                        return Redirect(url);
                    } else {
                        return RedirectToAction("Index");
                    }
                }
                claimant.Email = "";
                claimant.Persist = true;
                claimant.Provider = "Local";
                claimant.ReturnUrl = url;
                ViewData["BodyClass"] = "login-page";
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex.Message);
            }
            return View("SignIn", claimant);
        }

        // Login (Post)
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Login([FromForm] Claimant model) {
            try {
                await DNATools.Initialize();

                if (ModelState.IsValid) {
                    Login claimant = await Context.Login.Where(x => x.Email == model.Email).Where(x => x.Provider == model.Provider).Include(x => x.User).ThenInclude(x => x.Role).Include(x => x.User).ThenInclude(x => x.Status).SingleOrDefaultAsync();
                    if (claimant != null) {
                        if (claimant.User.Status.Code == "Pending") {
                            throw new ArgumentException("Please respond to the validation e-mail to activate your account");
                        } else {
                            if (claimant.Password == null) {
                                throw new Exception("Local athentication attempts should not have a null password");
                            } else {
                                if (Utilities.Security.ValidatePassword(model.Password, claimant.Password)) {
                                    var claims = Common.GetClaimsList(claimant);
                                    var userIdentity = new ClaimsIdentity(claims, "login");
                                    ClaimsPrincipal principal = new(userIdentity);
                                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                    //Log.Logger.ForContext("UserId", claimant.UserId).Warning($"Signed In ({claimant.Provider})");
                                    if (!String.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl)) {
                                        return Redirect(model.ReturnUrl);
                                    } else {
                                        return RedirectToAction("Index");
                                    }

                                } else {
                                    throw new ArgumentException("Please check your credentials and try again");
                                }
                            }
                        }
                    } else {
                        throw new ArgumentException("Please register a new membership to continue");
                    }
                }
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex.Message);
            }
            ViewData["BodyClass"] = "login-page";
            return View("SignIn", model);
        }

        // Register (Get)
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Register() {
            Registration instance = null;
            try {
                instance = new Registration() {
                    Terms = true,
                    Provider = "Local"
                };
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            ViewData["BodyClass"] = "register-page";
            return View("Register", instance);
        }

        // Register (Post)
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Register([FromForm] Registration registration) {
            if (ModelState.IsValid) {
                if (await Auth.RegisterAccountAsync(registration)) {
                    return RedirectToAction("Login");
                }
            }
            ViewData["BodyClass"] = "register-page";
            return View("Register", registration);
        }

        // Recover Password (Get)
        [HttpGet]
        [AllowAnonymous]
        [Authorize(Policy = "Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Recover() {
            Recover instance = null;
            try {
                instance = new Recover {
                    Provider = "Local"
                };
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            ViewData["BodyClass"] = "login-page";
            return View(instance);
        }

        // Recover Password (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Recover([FromForm] Recover model) {
            try {
                if (ModelState.IsValid) {
                    if (await Auth.RecoverPasswordAsync(model)) {
                        return RedirectToAction("Login");
                    }
                }
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex.Message);
            }
            ViewData["BodyClass"] = "login-page";
            return View("Recover", model);
        }

        // Password (Get)
        [HttpGet]
        [Authorize(Policy = "Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Password() {
            DNA3.Models.Password password = new() {

            };
            return View(password);
        }

        // Confirmation (Get)
        [HttpGet]
        [Authorize(Policy = "Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Confirmation(int id, [FromQuery] string Token) {
            try {
                Login login = await Context.Login.Include(x => x.User).ThenInclude(x => x.Client).Where(x => x.LoginId == id).FirstAsync() ?? throw new ArgumentException("User not found. Please sign up for a new account.");
                if (string.IsNullOrEmpty(login.User.Token)) {
                    throw new ArgumentException("Verification token not found. Manual navigation prohibited.");
                }
                if (login.User.TokenDate < DateTime.Now) {
                    throw new ArgumentException("Verification token expired. Please try again.");
                }
                if (Token == "") {
                    throw new ArgumentException("Verification token is empty. Manual navigation prohibited.");
                }
                if (Token == login.User.Token) {

                    // Generate Credentials
                    var password = Utilities.Security.GenerateRandomPassword();

                    // Store hashed Login Credentials
                    login.Password = Utilities.Security.CreateHash(password);

                    // Update User Status
                    login.User.StatusId = 10001;

                    // Save Changes
                    Context.Login.Update(login);
                    await Context.SaveChangesAsync();

                    // Send Welcome Message
                    var message = new MimeMessage();
                    message.From.Add(MailboxAddress.Parse(Configuration["Smtp:From"]));
                    message.To.Add(MailboxAddress.Parse(login.Email));
                    message.Subject = $"Welcome Aboard - {login.User.Client.Company}";
                    message.Body = new TextPart(TextFormat.Html) {
                        Text =
$@"
<p>Welcome Aboard!</p>

<p>Account Access:</p>
<p>    {Configuration["App:URL"]}/Dashboard/Login</p>
<p></p>
<p>Credentials:</p>
<p>    User Name:  {login.Email}</p>
<p>    Password:   {password}</p>
<p></p>
<p>If you have questions or need further assistance, please contact {Configuration["App:SupportAddress"]}</p>"
                    };

                    // Send Message
                    using MailKit.Net.Smtp.SmtpClient smtp = new();
                    smtp.Connect(Configuration["Smtp:Host"], Int16.Parse(Configuration["Smtp:Port"]), SecureSocketOptions.StartTls);
                    smtp.Authenticate(Configuration["Smtp:User"], Configuration["Smtp:Password"]);
                    smtp.Send(message);
                    smtp.Disconnect(true);
                    Site.Messages.Enqueue("Your request was sent successfully");

                    // Log Activity
                    Log.Logger.ForContext("UserId", login.UserId).Warning("Account Activated");

                    // Notification
                    Site.Messages.Enqueue("Account activated. Please check your E-Mail.");

                }
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex.Message);
                return RedirectToAction("Registration");
            }
            return RedirectToAction("Login");
        }

        // Client (Get)
        [HttpGet]
        [Authorize(Policy = "Administrators")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Client() {
            try {
                var client = await Context.Client.FindAsync(Convert.ToInt32(User.ClientId())) ?? throw new ArgumentException("Client not found, please contact Support for assistance.");
                ViewData["User"] = await Context.User.Include(x => x.Role).Include(x => x.Status).Where(x => x.UserId == User.UserId()).FirstOrDefaultAsync();
                ViewData["Users"] = await Context.User.Include(x => x.Role).Include(x => x.Status).Where(x => x.ClientId == User.ClientId()).OrderBy(x => x.First).OrderBy(x => x.Last).ToListAsync();
                return View(client);
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }

        // Client (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrators")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Client(Client client) {
            String message = "";
            try {
                if (ModelState.IsValid) {
                    Context.Client.Update(client);
                    await Context.SaveChangesAsync();
                    message = "Client Updated Successfully";
                    Site.Messages.Enqueue(message);
                    Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                }
                ViewData["User"] = await Context.User.Include(x => x.Role).Include(x => x.Status).Where(x => x.UserId == User.UserId()).FirstOrDefaultAsync();
                ViewData["Users"] = await Context.User.Where(x => x.ClientId == User.ClientId()).OrderBy(x => x.First).OrderBy(x => x.Last).ToListAsync();
                return View("Client", client);
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }

        // Profile (Get)
        [HttpGet]
        [Authorize(Policy = "Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Profile() {
            try {
                User user = await Context.User.FindAsync(Convert.ToInt32(User.UserId())) ?? throw new ArgumentException("User not found.");
                ViewBag.RoleList = await Context.Role.OrderBy(x => x.Description).ToListAsync();
                ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
                ViewData["Logins"] = await Context.Login.Where(x => x.UserId == User.UserId()).ToListAsync();
                ViewData["Client"] = await Context.Client.FindAsync(user.ClientId);

                // Two Factor QR Code
                if (user.TotpKey == null) {
                    user.TotpKey = Guid.NewGuid().ToString().Replace("-", "")[..10];
                    TwoFactorAuthenticator tfa = new();
                    SetupCode setupInfo = tfa.GenerateSetupCode("Click Tick Done", User.EmailAddress(), user.TotpKey, false, 3);
                    ViewData["QRCode"] = setupInfo.QrCodeSetupImageUrl;
                    user.TotpManualSetup = setupInfo.ManualEntryKey;
                }

                return View("Profile", user);
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }

        // Profile (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Profile(User user) {
            String message = "";
            try {
                if (ModelState.IsValid) {
                    if (user.TotpCode != null) {
                        TwoFactorAuthenticator tfa = new();
                        bool result = tfa.ValidateTwoFactorPIN("Move this to identity management", user.TotpCode);
                    }
                    Context.User.Update(user);
                    await Context.SaveChangesAsync();
                    message = "Profile Updated Successfully";
                    Site.Messages.Enqueue(message);
                    Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                }
                ViewBag.RoleList = await Context.Role.OrderBy(x => x.Description).ToListAsync();
                ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
                ViewData["Logins"] = await Context.Login.Where(x => x.UserId == User.UserId()).OrderBy(x => x.Email).ToListAsync();
                ViewData["Client"] = await Context.Client.FindAsync(user.ClientId);
                return View("Profile", user);
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }

        // Notifications
        [HttpGet]
        [Authorize(Policy = "Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Notifications() {
            try {
                ViewData["Count"] = await Context.Activity.FromSqlInterpolated($"SELECT Id, Message, MessageTemplate, Level, TimeStamp, Exception, UserId FROM Activity WHERE UserId = {User.UserId()}").CountAsync();
                ViewBag.Notifications = await Context.Activity.FromSqlInterpolated($"SELECT TOP 12 Id, Message, MessageTemplate, Level, TimeStamp, Exception, UserId FROM Activity WHERE UserId = {User.UserId()} ORDER BY TimeStamp DESC").ToListAsync();
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex.Message);
            }
            return PartialView("_Notifications");
        }

        // Logout (Get)
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Logout() {
            try {
                await HttpContext.SignOutAsync();
                Log.Logger.ForContext("UserId", User.UserId()).Warning("Signed Out");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex.Message);
            }
            return Redirect("/");
        }

        #endregion

        #region Remote Validation Methods

        // Request Not Registered User
        [AllowAnonymous]
        public JsonResult RequestNotRegisteredUser(string Email) {
            return Json(!Registered(Email));
        }

        // Registration Not Registered User
        [AllowAnonymous]
        public JsonResult RegistrationNotRegisteredUser(string Email) {
            return Json(!Registered(Email));
        }

        #endregion

        #region Common Methods

        // Registered
        public bool Registered(string EmailAddress) {
            bool result = false;
            try {
                Login login = Context.Login.Where(x => x.Email == EmailAddress).FirstOrDefault();
                if (login != null) {
                    if (login.Email == EmailAddress) {
                        result = true;
                    }
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                result = false;
            }
            return result;
        }

        #endregion

    }

}
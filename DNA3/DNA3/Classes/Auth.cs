#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Utilities;

#endregion

namespace DNA3.Classes {

    #region Interface

    public interface IAuth {
        Task<string> AuthenticateCertificate(CertificateValidatedContext certcontext);
        Task<bool> RecoverPasswordAsync(Recover model);
        Task<bool> RegisterAccountAsync(Registration model);
        bool SmtpMessage(string from, string to, string subject, string content);
    }

    #endregion

    public class Auth(IConfiguration configuration, MainContext context, ILogger<Auth> logger, IHttpContextAccessor contextaccessor) : IAuth {

        #region Variables

        private readonly IConfiguration Configuration = configuration;
        private readonly MainContext Context = context;
        private readonly ILogger<Auth> Logger = logger;
        private readonly IHttpContextAccessor ContextAccessor = contextaccessor;

        #endregion

        #region Authentication

        // Authenticate Certificate
        public async Task<string> AuthenticateCertificate(CertificateValidatedContext certcontext) {
            string result = "";
            try {
                Login claimant = await Context.Login.Include(x => x.User).ThenInclude(x => x.Role).Include(x => x.User).ThenInclude(x => x.Status).Where(x => x.User.Thumbprint == certcontext.ClientCertificate.Thumbprint).FirstOrDefaultAsync();
                if (claimant != null) {
                    if (claimant.User.Status.Code == "Pending") {
                        throw new ArgumentException("Please respond to the identity validation e-mail to activate your account");
                    } else {
                        if (claimant.User.Status.Code == "Active") {
                            var claims = Common.GetClaimsList(claimant);
                            var userIdentity = new ClaimsIdentity(claims, "login");
                            ClaimsPrincipal principal = new(userIdentity);
                            certcontext.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, certcontext.Scheme.Name));
                            certcontext.Success();
                            Log.Logger.Information($"Certificate Authenticated ({claimant.User.FullName})");
                            result = "Success";
                        } else {
                            throw new ArgumentException($"Account is {claimant.User.Status.Code}");
                        }
                    }
                } else {
                    throw new ArgumentException("Certificate not found");
                }
            } catch (Exception ex) {
                result = ex.Message;
                certcontext.Fail(result);
                Site.Messages.Enqueue(result);
                Logger.LogError(ex, "{message}", result);
            }
            return result;
        }

        // Recover Password
        public async Task<bool> RecoverPasswordAsync(Recover model) {
            bool result = true;
            try {
                Login claimant = await Context.Login.Where(x => x.Email == model.Email).Where(x => x.Provider == model.Provider).Include(x => x.User).ThenInclude(x => x.Client).Include(x => x.User).ThenInclude(x => x.Role).Include(x => x.User).ThenInclude(x => x.Status).SingleOrDefaultAsync();
                if (claimant != null) {

                    var token = Utilities.Security.CreateHash(Guid.NewGuid().ToString());

                    using (var c = new MainContext(new DbContextOptions<MainContext>(), Configuration)) {
                        var user = await c.User.Where(x => x.UserId == claimant.UserId).SingleOrDefaultAsync();
                        user.Token = token;
                        user.TokenDate = DateTime.Now.AddMinutes(60);
                        c.Update(user);
                        await c.SaveChangesAsync();
                    }

                    var message = new MailMessage(
                        $"{Configuration["App:ShortName"]} <{Configuration["Smtp:From"]}>",
                        model.Email,
                        $"Password Recovery - {claimant.User.Client.Company}",
                        $@"
Please click the link below and you'll arrive at a page where you can set a secure password for your account. 
If you're unable to click the link, please copy and paste the information into your browser address bar:

{ContextAccessor.HttpContext.Request.Scheme}://{ContextAccessor.HttpContext.Request.Host}/Dashboard/Confirmation/{claimant.LoginId}?Token={HttpUtility.UrlEncode(token)}

Please note that this link will expire one hour from the time it was issued.

If you have questions or need further assistance, please contact {Configuration["App:SupportName"]}"
                    );

                    var credentials = new NetworkCredential(Configuration["Smtp:User"], Configuration["Smtp:Password"]);

                    var smtpclient = new SmtpClient {
                        Host = Configuration["Smtp:Host"],
                        Port = Convert.ToInt16(Configuration["Smtp:Port"]),
                        Credentials = credentials,
                        EnableSsl = true
                    };

                    smtpclient.Send(message);

                    Site.Messages.Enqueue("Please check your E-Mail to continue.");
                } else {
                    Site.Messages.Enqueue("E-Mail address not found.");
                }
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return result;
        }

        // Register Account
        public async Task<bool> RegisterAccountAsync(Registration model) {
            bool result = true;
            try {

                Login claimant = await Context.Login.Where(x => x.Email == model.Email).Where(x => x.Provider == model.Provider).Include(x => x.User).ThenInclude(x => x.Role).Include(x => x.User).ThenInclude(x => x.Status).SingleOrDefaultAsync();

                if (claimant != null) {
                    Site.Messages.Enqueue("We already have your e-mail address on file.");
                    result = true; // Returning true here will redirect to Login
                } else {

                    var token = Utilities.Security.CreateHash(Guid.NewGuid().ToString());

                    var client = new Client {
                        Company = model.Company,
                        Address1 = "",
                        Address2 = "",
                        City = "",
                        State = "",
                        Zip = "",
                        Zip1 = "",
                        Phone = "",
                        Avitar = "",
                        StatusId = Ado.GetScalarValue<int>(Configuration["ConnectionStrings:MainContext"], "StatusId", "Status", "Code", "Active"),
                        Comment = $"Added via Account Registration Manager on {DateTime.Now}",
                        Users = []
                    };

                    var user = new User {
                        ClientId = client.ClientId,
                        First = model.First,
                        Last = model.Last,
                        StatusId = Ado.GetScalarValue<int>(Configuration["ConnectionStrings:MainContext"], "StatusId", "Status", "Code", "Pending"),
                        RoleId = Ado.GetScalarValue<int>(Configuration["ConnectionStrings:MainContext"], "RoleId", "Role", "Code", "Reseller"),
                        Persist = true,
                        Token = token,
                        TokenDate = DateTime.Now.AddMinutes(60),
                        Logins = []
                    };

                    // Respects Password Options object configured in Startup.cs
                    var login = new Login {
                        Provider = "Local",
                        UserId = user.UserId,
                        Email = model.Email,
                        Password = Utilities.Security.GenerateRandomPassword()
                    };

                    user.Logins.Add(login);

                    client.Users.Add(user);

                    Context.Client.Add(client);

                    await Context.SaveChangesAsync();

                    var message = new MailMessage(
                        Configuration["Smtp:From"],
                        model.Email,
                        "Account Confirmation",
                        $@"
Please click on the link below to confirm your identity and complete the registration process. 
If you're unable to click the link, please copy and paste the information into your browser address bar:

{ContextAccessor.HttpContext.Request.Scheme}://{ContextAccessor.HttpContext.Request.Host}/Dashboard/Confirmation/{login.LoginId}?Token={HttpUtility.UrlEncode(token)}

Please note that this link will expire one hour from the time it was issued.

If you have questions or need further assistance, please contact {Configuration["App:SupportName"]}"
                    );

                    var credentials = new NetworkCredential(Configuration["Smtp:User"], Configuration["Smtp:Password"]);

                    var smtpclient = new SmtpClient {
                        Host = Configuration["Smtp:Host"],
                        Port = Convert.ToInt16(Configuration["Smtp:Port"]),
                        Credentials = credentials,
                        EnableSsl = true
                    };

                    smtpclient.Send(message);

                    Site.Messages.Enqueue("Please check your E-Mail.");
                }

            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
                result = false;
            }
            return result;
        }

        #endregion

        #region Common Methods

        // Smtp Message
        public bool SmtpMessage(string from, string to, string subject, string content) {
            bool result = true;
            try {
                var message = new MailMessage(from, to, subject, content);
                var credentials = new NetworkCredential(Configuration["Smtp:User"], Configuration["Smtp:Password"]);
                var client = new SmtpClient {
                    Host = Configuration["Smtp:Host"],
                    Port = Convert.ToInt16(Configuration["Smtp:Port"]),
                    Credentials = credentials,
                    EnableSsl = true
                };
                client.Send(message);
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
                result = false;
            }
            return result;
        }

        #endregion

    }

}

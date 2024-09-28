#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Authentication;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;
using Utilities;

#endregion

namespace DNA3.Classes {

    #region Interface

    #endregion

    #region Class

    public class Handlers {

        #region Authentication

        public static async System.Threading.Tasks.Task GoogleOnTicketReceived(TicketReceivedContext context) {
            try {
                Login claimant = Utilities.Ado.ListFromSql<Login>(Utilities.Site.ConnectionString, new string[] { $"@Email={context.Principal.EmailAddress}" }, "SELECT * FROM [Login] AS l WHERE l.Email = @Email" ).FirstOrDefault();
                if (claimant != null) {
                    if (claimant.User.Status.Code == "Pending") {
                        throw new ArgumentException("Please respond to the identity validation e-mail to activate your account");
                    } else {
                        if (claimant.User.Status.Code == "Active") {
                            var claims = Common.GetClaimsList(claimant);
                            var userIdentity = new ClaimsIdentity(claims, "login");
                            ClaimsPrincipal principal = new(userIdentity);
                            context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                            context.Success();
                            Log.Logger.Information($"Azure Account Authenticated ({claimant.User.FullName})");
                        } else {
                            throw new ArgumentException($"Azure Account is {claimant.User.Status.Code}");
                        }
                    }
                } else {
                    throw new ArgumentException("Azure Account not found");
                }
            } catch(Exception ex) {
                Log.Error(ex, ex.Message);
            }
        }

        #endregion

    }

    #endregion

}

#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
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
                Claimant claimant = Utilities.Ado.ListFromSql<DNA3.Models.Claimant>(Utilities.Site.ConnectionString, [$"@Email={context.Principal.EmailAddress()}"], "SELECT l.LoginId, l.Provider, l.UserId, u.ClientId, l.Email, u.First, u.Last, s.Code AS StatusCode, r.Code as RoleCode FROM [Login] AS l INNER JOIN [User] AS u ON l.UserId = u.UserId INNER JOIN [Status] AS s ON u.StatusId = s.StatusId INNER JOIN [Role] AS r ON u.RoleId = r.RoleId WHERE l.Provider = 'Google' AND l.Email = @Email").FirstOrDefault();
                if (claimant == null) {
                    string email = context.Principal.FindFirst(ClaimTypes.Email).Value;
                    string first = context.Principal.FindFirst(ClaimTypes.GivenName).Value;
                    string last = context.Principal.FindFirst(ClaimTypes.Surname).Value;
                    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(first) || string.IsNullOrEmpty(last)) {
                        throw new ArgumentException("Your Google account must specify your e-mail address along with your First and Last Name");
                    } else {
                        Utilities.Ado.CreateNewAccount(Utilities.Site.ConnectionString, email, first, last);
                    }
                    claimant = Utilities.Ado.ListFromSql<DNA3.Models.Claimant>(Utilities.Site.ConnectionString, [$"@Email={context.Principal.EmailAddress()}"], "SELECT l.LoginId, l.Provider, l.UserId, u.ClientId, l.Email, u.First, u.Last, s.Code AS StatusCode, r.Code as RoleCode FROM [Login] AS l INNER JOIN [User] AS u ON l.UserId = u.UserId INNER JOIN [Status] AS s ON u.StatusId = s.StatusId INNER JOIN [Role] AS r ON u.RoleId = r.RoleId WHERE l.Provider = 'Google' AND l.Email = @Email").FirstOrDefault();
                }
                if (claimant != null) {
                    if (claimant.StatusCode == "Pending") {
                        throw new ArgumentException("Please respond to the identity validation e-mail to activate your account");
                    } else {
                        if (claimant.StatusCode == "Active") {
                            var claims = new List<Claim> {
                                new("lgnid", claimant.LoginId.ToString()),
                                new("usrid", claimant.UserId.ToString()),
                                new("cliid", claimant.ClientId.ToString()),
                                new("first", claimant.First),
                                new("last", claimant.Last),
                                new("full", claimant.First + ' ' + claimant.Last),
                                new("email", claimant.Email),
                                new("role", claimant.RoleCode),
                                new(ClaimTypes.Email, claimant.Email)
                            };
                            var userIdentity = new ClaimsIdentity(claims, "login");
                            ClaimsPrincipal principal = new(userIdentity);
                            context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                            context.Success();
                            Log.Logger.Information($"Google Account Authenticated (claimant.First + ' ' + claimant.Last)");
                        } else {
                            throw new ArgumentException($"Google Account is {claimant.User.Status.Code}");
                        }
                    }
                }
            } catch(Exception ex) {
                Log.Error(ex, ex.Message);
            }
        }

        #endregion

    }

    #endregion

}

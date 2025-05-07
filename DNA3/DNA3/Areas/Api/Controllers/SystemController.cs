#region Usings

using DNA3.Classes;
using DNA3.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Utilities;

#endregion

namespace DNA3.API {

	/// <summary>
	/// Provides authentication and user management services
	/// </summary>
	[Authorize(Policy = "ApiUserPolicy", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class SystemController : Controller {

        #region Properties, Variables and Constants

        private readonly MainContext Context;
        private readonly IConfiguration Configuration;
        private readonly ILogger<SystemController> Logger;
		readonly string ConnectionString = "";

        #endregion

        #region Class Methods

        public SystemController(MainContext context, IConfiguration configuration, ILogger<SystemController> logger) {
            Context = context;
            Configuration = configuration;
            Logger = logger;
            ConnectionString = Configuration.GetConnectionString("MainContext");
        }

        #endregion

        #region Controller Actions
        [AllowAnonymous]
        [HttpGet]
        [Route("api/getthemepath")]
        public ContentResult GetThemePath() {
            string result;
            try {
                result = Configuration["App:Themepath"];
            } catch {
                result = "";
            }
            return base.Content(result, "text/html", Encoding.UTF8);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/authenticate")]
        public IActionResult Authenticate([FromBody] Claimant model) {
            Claimant claimant = new();
            try {

                var ClaimantList = Context.Claimant.FromSqlRaw("GetClaimant {0}, {1}", model.Email, model.Provider).ToList();
                claimant = ClaimantList.SingleOrDefault();

                if (claimant != null) {

                    if (Common.ValidatePassword(model.Password, claimant.Password)) {

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);
                        var expiration = DateTime.UtcNow.AddDays(Convert.ToDouble(Configuration["Jwt:ExpireDays"]));
                        var tokenDescriptor = new SecurityTokenDescriptor {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                            new Claim(ClaimTypes.Name, claimant.UserId.ToString()),
                            new Claim(ClaimTypes.Email, claimant.Email),
                            new Claim("Full", string.Format("{0} {1}", claimant.First, claimant.Last)),
                            new Claim("First", claimant.First),
                            new Claim("Last", claimant.Last),
                            new Claim("Role", claimant.RoleCode)
                            }),
                            Expires = expiration,
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);

                        // Return basic user information with token to store client-side
                        claimant.Token = tokenHandler.WriteToken(token);
                        claimant.Tokendate = expiration;

                    } else {
                        throw new ArgumentException("Password does not match our records");
                    }

                } else {
                    throw new ArgumentException("E-Mail Address not found.");
                }

            } catch (Exception ex) {
                Logger.LogError(ex.Message);
                claimant.StatusMessage = ex.Message;
                return new ContentResult {
                    Content = JsonConvert.SerializeObject(claimant),
                    ContentType = "application/json",
                    StatusCode = 401
                };
            }

            claimant.StatusMessage = "Success";
            return new ContentResult {
                Content = JsonConvert.SerializeObject(claimant),
                ContentType = "application/json",
                StatusCode = 200
            };

        }

        [Route("api/system/statuschanges/{id}")]
        [HttpGet]
        public IActionResult StatusChanges(int id) {

            // Create Dataset
            DataSet ds = new("Remote");

            try {

                using (SqlConnection conn = new(ConnectionString)) {

                    // Add Status Table
                    using (SqlCommand cmd = new("SELECT * FROM [Status] WHERE sta_id > @Id", conn)) {
                        using (SqlDataAdapter da = new(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.SelectCommand.Parameters.AddWithValue("@Id", id);
                            da.Fill(ds, "Status");
                            Ado.SetAdded(ref ds, "Status");
                        }
                    }

                    // Add Modified Status Codes
                    using (SqlCommand cmd = new("SELECT * FROM [Status] WHERE sta_id <= @Id AND sta_modified = 1", conn)) {
                        using (SqlDataAdapter da = new(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.SelectCommand.Parameters.AddWithValue("@Id", id);
                            da.Fill(ds, "Status");
                            Ado.SetModified(ref ds, "Status");
                        }
                    }

                }

            } catch (SqlException ex) {
                Debug.WriteLine("Errors Count:" + ex.Errors.Count);
                foreach (SqlError sqlerror in ex.Errors) {
                    Logger.LogError(sqlerror.Number, sqlerror.Message);
                    Debug.WriteLine(sqlerror.Number + " - " + sqlerror.Message);
                }
            } catch (Exception ex) {
                Logger.LogInformation(String.Format("[System Controller] - Status Changes: {0}", ex.Message));
            }

            // Return completed Dataset with OK(200) Status
            return Ok(ds);
        }

        [Route("api/system/userchanges/{id}")]
        [HttpGet]
        public IActionResult UserChanges(int id) {

            // Create Dataset
            DataSet ds = new("Remote");

            try {

                using (SqlConnection conn = new(ConnectionString)) {

                    // Add User Table
                    using (SqlCommand cmd = new("SELECT * FROM [User] WHERE usr_id > @Id", conn)) {
                        using (SqlDataAdapter da = new(cmd)) {
                            //da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.SelectCommand.Parameters.AddWithValue("@Id", id);
                            da.Fill(ds, "User");
                            Ado.SetAdded(ref ds, "User");
                        }
                    }

                    // Add Modified Users
                    using (SqlCommand cmd = new("SELECT * FROM [User] WHERE usr_id <= @Id AND usr_modified = 1", conn)) {
                        using (SqlDataAdapter da = new(cmd)) {
                            //da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.SelectCommand.Parameters.AddWithValue("@Id", id);
                            da.Fill(ds, "User");
                            Ado.SetModified(ref ds, "User");
                        }
                    }

                }

            } catch (SqlException ex) {
                Debug.WriteLine("Errors Count:" + ex.Errors.Count);
                foreach (SqlError sqlerror in ex.Errors) {
                    Logger.LogError(sqlerror.Number, sqlerror.Message);
                    Debug.WriteLine(sqlerror.Number + " - " + sqlerror.Message);
                }
            } catch (Exception ex) {
                Logger.LogInformation(String.Format("[System Controller] - User Changes: {0}", ex.Message));
            }

            // Return completed Dataset with OK(200) Status
            return Ok(ds);
        }

        [Route("api/system/loginchanges/{id}")]
        [HttpGet]
        public IActionResult LoginChanges(int id) {

            // Create Dataset
            DataSet ds = new("Remote");

            try {

                using (SqlConnection conn = new(ConnectionString)) {

                    // Add Login Table
                    using (SqlCommand cmd = new("SELECT * FROM [Login] WHERE lgn_id > @Id", conn)) {
                        using (SqlDataAdapter da = new(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.SelectCommand.Parameters.AddWithValue("@Id", id);
                            da.Fill(ds, "Login");
                            Ado.SetAdded(ref ds, "Login");
                        }
                    }

                    // Add Modified Logins
                    using (SqlCommand cmd = new("SELECT * FROM [Login] WHERE lgn_id <= @Id AND lgn_modified = 1", conn)) {
                        using (SqlDataAdapter da = new(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.SelectCommand.Parameters.AddWithValue("@Id", id);
                            da.Fill(ds, "Login");
                            Ado.SetModified(ref ds, "Login");
                        }
                    }

                }

            } catch (SqlException ex) {
                Debug.WriteLine("Errors Count:" + ex.Errors.Count);
                foreach (SqlError sqlerror in ex.Errors) {
                    Logger.LogError(sqlerror.Number, sqlerror.Message);
                    Debug.WriteLine(sqlerror.Number + " - " + sqlerror.Message);
                }
            } catch (Exception ex) {
                Logger.LogInformation(String.Format("[System Controller] - Login Changes: {0}", ex.Message));
            }

            // Return completed Dataset with OK(200) Status
            return Ok(ds);
        }

        [AllowAnonymous]
        [Route("api/system/remoteipaddress")]
        [HttpGet]
        public IActionResult RemoteIPAddress() {
            String IPAddress = null;
            try {
                IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            } catch (SqlException ex) {
                Debug.WriteLine("Errors Count:" + ex.Errors.Count);
                foreach (SqlError sqlerror in ex.Errors) {
                    Logger.LogError(sqlerror.Number, sqlerror.Message);
                    Debug.WriteLine(sqlerror.Number + " - " + sqlerror.Message);
                }
            } catch (Exception ex) {
                Logger.LogInformation(String.Format("[System Controller] - Login Changes: {0}", ex.Message));
            }

            // Return completed Dataset with OK(200) Status
            return Ok(IPAddress);
        }

        #endregion

    }

}

#region Usings

using DNA3.Classes;
using DNA3.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

#endregion

namespace DNA3.API {

	/// <summary>
	/// AdoUserController()
	/// </summary>
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class AdoUserController : Controller {

        #region Properties, Variables and Constants

        private MainContext Context;
        private IConfiguration Configuration;
        private ILogger<AdoUserController> Logger;
        string ConnectionString = "";

        #endregion

        #region Class Methods

        public AdoUserController(MainContext context, IConfiguration configuration, ILogger<AdoUserController> logger) {
            Context = context;
            Configuration = configuration;
            Logger = logger;
            ConnectionString = Configuration.GetConnectionString("MainContext");
        }

        #endregion

        #region Controller Actions

        /// <summary>
        /// Download
        /// </summary>
        /// <param name="id">The greatest identity value existing in the remote database</param>
        /// <returns>Provides a DataSet containing all new Users having an identity value greater than [id] and all users where usr_modified is true and its identity value is less than or equal to [id]</returns>
        [Route("api/adouser/download")]
        [HttpGet]
        public IActionResult Download(int id) {

            // Create Dataset
            DataSet ds = new DataSet("Remote");

            try {

                using (SqlConnection conn = new SqlConnection(ConnectionString)) {

                    // Add User Table
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [User] WHERE usr_id > @Id", conn)) {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.SelectCommand.Parameters.AddWithValue("@Id", id);
                            da.Fill(ds, "User");
                            Data.SetAdded(ref ds, "User");
                        }
                    }

                    // Add Modified Users
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [User] WHERE usr_id <= @Id AND usr_modified = 1", conn)) {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.SelectCommand.Parameters.AddWithValue("@Id", id);
                            da.Fill(ds, "User");
                            Data.SetModified(ref ds, "User");
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

        /// <summary>
        /// Returns a dataset containing the most recent data for all Users
        /// </summary>
        /// <returns>Users and Status</returns>
        [Route("api/adouser/list")]
        [HttpGet]
        public IActionResult List() {

            DataSet ds = new DataSet("Remote");

            try {
                using (SqlConnection conn = new SqlConnection(ConnectionString)) {

                    // Add Status Table
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [Status]", conn)) {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.Fill(ds, "Status");
                        }
                    }

                    // Add Role Table
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [Role]", conn)) {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.Fill(ds, "Role");
                        }
                    }

                    // Add User Table
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [User]", conn)) {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.Fill(ds, "User");
                        }
                    }

                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }

            return Ok(ds);

        }

        /// <summary>
        /// User New
        /// </summary>
        /// <returns>Dataset containing a new User and all supporting tables</returns>
        [Route("api/adouser/new")]
        [HttpGet]
        public IActionResult New() {

            DataSet ds = new DataSet("Remote");

            try {

                using (SqlConnection conn = new SqlConnection(ConnectionString)) {

                    // Add Client Table
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [Client]", conn)) {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.Fill(ds, "Client");
                        }
                    }


                    // Add Role Table
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [Role]", conn)) {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.Fill(ds, "Role");
                        }
                    }

                    // Add Status Table
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [Status]", conn)) {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.Fill(ds, "Status");
                        }
                    }

                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }

            return Ok(ds);

        }

        /// <summary>
        /// User Open
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Dataset containing a specific User and all supporting tables</returns>
        [Route("api/adouser/open/{id}")]
        [HttpGet]
        public IActionResult Open(int id) {

            DataSet ds = new DataSet("Remote");

            try {

                using (SqlConnection conn = new SqlConnection(ConnectionString)) {

                    // Add Client Table
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [Client]", conn)) {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.Fill(ds, "Client");
                        }
                    }

                    // Add Role Table
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [Role]", conn)) {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.Fill(ds, "Role");
                        }
                    }

                    // Add Status Table
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [Status]", conn)) {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.Fill(ds, "Status");
                        }
                    }

                    // Add Login Table
                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM [Login] WHERE lgn_usrid = {id}", conn)) {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.Fill(ds, "Login");
                        }
                    }

                    // Add User Table
                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM [User] WHERE usr_id = {id}", conn)) {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.Fill(ds, "User");
                        }
                    }

                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }

            return Ok(ds);

        }

        /// <summary>
        /// User Save
        /// </summary>
        /// <param name="ds">Dataset containing the Users to save.</param>
        /// <returns>HTTP Reponse Message with most recent Identity value as an output parameter</returns>
        [Route("api/adouser/save")]
        [HttpPost]
        public IActionResult Save([FromBody] DataSet ds) {
            SqlParameter spId;
            SqlParameter spResponse;
            int? result = 0;
            try {
                using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                    SqlDataAdapter daUser = new SqlDataAdapter("SELECT * FROM [User]", conn) {
                        MissingSchemaAction = MissingSchemaAction.AddWithKey
                    };
                    SqlCommandBuilder cbUser = new SqlCommandBuilder(daUser);
                    daUser.UpdateCommand = cbUser.GetUpdateCommand().Clone();
                    daUser.DeleteCommand = cbUser.GetDeleteCommand().Clone();
                    daUser.InsertCommand = new SqlCommand("UserInsert", conn) {
                        CommandType = CommandType.StoredProcedure
                    };
                    daUser.InsertCommand.Parameters.Add("@usr_cliid", SqlDbType.Int, 0, "usr_cliid");
                    daUser.InsertCommand.Parameters.Add("@usr_first", SqlDbType.VarChar, 30, "usr_first");
                    daUser.InsertCommand.Parameters.Add("@usr_last", SqlDbType.VarChar, 30, "usr_last");
                    daUser.InsertCommand.Parameters.Add("@usr_title", SqlDbType.VarChar, 30, "usr_title");
                    daUser.InsertCommand.Parameters.Add("@usr_name", SqlDbType.VarChar, 30, "usr_name");
                    daUser.InsertCommand.Parameters.Add("@usr_locid", SqlDbType.Int, 0, "usr_locid");
                    daUser.InsertCommand.Parameters.Add("@usr_dptid", SqlDbType.Int, 0, "usr_dptid");
                    daUser.InsertCommand.Parameters.Add("@usr_srcid", SqlDbType.VarChar, 30, "usr_srcid");
                    daUser.InsertCommand.Parameters.Add("@usr_email", SqlDbType.VarChar, 160, "usr_email");
                    daUser.InsertCommand.Parameters.Add("@usr_password", SqlDbType.VarChar, 100, "usr_password");
                    daUser.InsertCommand.Parameters.Add("@usr_note", SqlDbType.VarChar, 300, "usr_note");
                    daUser.InsertCommand.Parameters.Add("@usr_rolid", SqlDbType.Int, 0, "usr_rolid");
                    daUser.InsertCommand.Parameters.Add("@usr_role", SqlDbType.VarChar, 30, "usr_role");
                    daUser.InsertCommand.Parameters.Add("@usr_persist", SqlDbType.Int, 0, "usr_persist");
                    daUser.InsertCommand.Parameters.Add("@usr_card", SqlDbType.VarChar, 4, "usr_card");
                    daUser.InsertCommand.Parameters.Add("@usr_staid", SqlDbType.Int, 0, "usr_staid");
                    daUser.InsertCommand.Parameters.Add("@usr_token", SqlDbType.VarChar, 128, "usr_token");
                    daUser.InsertCommand.Parameters.Add("@usr_tokendate", SqlDbType.DateTime, 0, "usr_tokendate");
                    daUser.InsertCommand.Parameters.Add("@usr_identity", SqlDbType.NVarChar, 128, "usr_identity");
                    spId = daUser.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0);
                    spId.Direction = ParameterDirection.Output;
                    spResponse = daUser.InsertCommand.Parameters.Add("@Response", SqlDbType.VarChar, 300);
                    spResponse.Direction = ParameterDirection.Output;
                    cbUser.Dispose();
                    daUser.Update(ds, "User");
                    result = (int?)daUser.InsertCommand.Parameters["@ID"].Value ?? 0;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }
            return Ok(result);
        }

        /// <summary>
        /// User Save
        /// </summary>
        /// <param name="ds">Dataset containing the Users to add, edit or delete.</param>
        /// <returns>HTTP Reponse Message</returns>
        [Route("api/adouser/saveall")]
        [HttpPost]
        public IActionResult SaveAll([FromBody] DataSet ds) {
            try {
                using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                    SqlDataAdapter daUser = new SqlDataAdapter("SELECT * FROM [User]", conn) {
                        MissingSchemaAction = MissingSchemaAction.AddWithKey
                    };
                    SqlCommandBuilder cbUser = new SqlCommandBuilder(daUser);
                    daUser.Update(ds, "User");
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }
            return Ok();
        }

        /// <summary>
        /// User Delete
        /// </summary>
        /// <param name="id">The ID of the User to be deleted.</param>
        /// <returns>HTTP Response Message</returns>
        [Route("api/adouser/delete/{id}")]
        [HttpPost]
        public IActionResult Delete(int id) {
            try {
                using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                    using (SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "StatementDelete";
                        cmd.CommandTimeout = 30;
                        cmd.Parameters.AddWithValue("@StatementId", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }
            return Ok("Success");
        }

        [Route("api/adouser/report")]
        [HttpGet]
        public IActionResult Report() {
            var ds = new DataSet();
            try {
                using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                    using (SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "UserListReport";
                        cmd.CommandTimeout = 30;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            da.Fill(ds, "UserListReport");
                        }
                    }
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }
            return Ok(ds);
        }
    }

    #endregion

}


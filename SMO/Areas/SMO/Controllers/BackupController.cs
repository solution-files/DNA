#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace SMO.Controllers {

    [Area("SMO")]
    [Authorize(Policy = "Administrators")]
    public class BackupController : Controller {

        #region Properties and Variables

        private readonly IConfiguration Configuration;
        private readonly MainContext Context;
        private readonly ILogger<BackupController> Logger;

        #endregion

        #region Class Methods

        // Constructor
        public BackupController(IConfiguration configuration, MainContext context, ILogger<BackupController> logger) {
            Configuration = configuration;
            Context = context;
            Logger = logger;
        }

        #endregion

        #region Controller Methods

        // Index (Get)
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Index() {
            string message;
            try {
                IList<Databases> result = await Context.Databases.FromSqlRaw<DNA3.Models.Databases>("SELECT database_id, name, user_access_desc, create_date FROM sys.databases WHERE name NOT IN('master', 'tempdb', 'model', 'msdb')").ToListAsync();
                Log.Logger.ForContext("UserId", User.UserId()).Information($"View Database List");
                return View("Index", result);
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        // Help
        [HttpGet, HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Help() {
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Information($"View Database Backup Help");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return View();
        }

        // Run (Post) - IMPORTANT: Even though the instance is a complex type, the action below will not function correctly without specifying the [FromBody] binding.
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Run([FromBody] IList<Databases> instance) {
            try {
                ServerConnection cn = new ServerConnection() {
                    ServerInstance = Configuration["SMOSettings:ServerInstance"],
                    LoginSecure = Convert.ToBoolean(Configuration["SMOSettings:LoginSecure"]),
                    Login = Configuration["SMOSettings:Login"],
                    Password = Configuration["SMOSettings:Password"]
                };
                cn.Connect();
                foreach (Databases database in instance) {
                    Server srv = new Server(cn);
                    Database db = default;
                    db = srv.Databases[database.name];
                    int recoverymod;
                    recoverymod = (int)db.DatabaseOptions.RecoveryModel;
                    Backup bk = new Backup() {
                        Action = BackupActionType.Database,
                        BackupSetDescription = $"Full backup of {database.name}",
                        BackupSetName = $"{database.name} Backup",
                        Database = $"{database.name}"
                    };

                    BackupDeviceItem bdi = default;
                    bdi = new BackupDeviceItem($"{Utilities.Strings.BackupFileName(database.name)}.bak", DeviceType.File);

                    bk.Devices.Add(bdi);
                    bk.Incremental = false;

                    System.DateTime backupdate = new System.DateTime();
                    backupdate = DateTime.Now;
                    bk.ExpirationDate = backupdate.AddMonths(12);
                    bk.LogTruncation = BackupTruncateLogType.Truncate;
                    bk.SqlBackup(srv);
                    bk.Devices.Remove(bdi);

                }
                Site.Messages.Enqueue("Full backup of all selected databases initiated successfully");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return Json(true);
        }

        #endregion

    }

}

#region Usings

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Smo;
using MimeKit;
using Serilog;
using SMO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

#endregion

namespace SMO.Controllers {

    [Area("SMO")]
    [Authorize(Policy = "Administrators")]
    public class DownloadController : Controller {

        #region Properties and Variables

        private readonly IConfiguration Configuration;
        private readonly ILogger<DownloadController> Logger;

        #endregion

        #region Class Methods

        // Constructor
        public DownloadController(IConfiguration configuration, ILogger<DownloadController> logger) {
            Configuration = configuration;
            Logger = logger;
        }

        #endregion

        #region Controller Methods

        // Index (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Index() {
            List<SMO.Models.Backup> files = new();
            try {
                string[] f = Directory.GetFiles(Path.Combine(Utilities.FileServer.GetUNCPath("/private/backups"), @Configuration["SMOSettings:BackupStoragePath"]), "*.bak");
                foreach (string file in f) {
                    files.Add(new SMO.Models.Backup { FileName = Path.GetFileName(file) });
                }
                files = files.OrderByDescending(x => x.FileName).ToList();
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                Site.Messages.Enqueue(ex.Message);
            }
            return View(files);
        }

        // Help
        [HttpGet, HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Help() {
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Information($"View Database Download Help");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return View();
        }

        // Stream (Post)
        [HttpGet, HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Stream([FromBody] IList<SMO.Models.Backup> backups) {
            string path = @Configuration["SMOSettings:BackupStoragePath"];
            string filePath = "";
            try {
                foreach(SMO.Models.Backup b in backups) {
                    Response.Headers.Clear();
                    filePath = $@"{path}{b.FileName}";
                    Response.Headers["Content-Disposition"] = @$"attachment;filename={filePath}";
                    return PhysicalFile(filePath, MimeTypes.GetMimeType(filePath), Path.GetFileName(filePath));
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                Site.Messages.Enqueue(ex.Message);
            }
            return Json(false);
        }

        #endregion

    }

}

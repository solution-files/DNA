#region Usings

using DNA3.Models;
using DOC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

#endregion

namespace SMO.Controllers {

    [Area("DOC")]
    [Authorize(Policy = "Administrators")]
    public class DownloadController : Controller {

        #region Properties and Variables

        private readonly IWebHostEnvironment Environment;
        private readonly MainContext Context;
        private readonly ILogger<DownloadController> Logger;
        private readonly IConfiguration Configuration;

        #endregion

        #region Class Methods

        // Constructor
        public DownloadController(IConfiguration configuration, IWebHostEnvironment environment, MainContext context, ILogger<DownloadController> logger) {
            Configuration = configuration;
            Environment = environment;
            Context = context;
            Logger = logger;
        }

        #endregion

        #region Controller Methods

        // Index (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Index() {
            List<Download> files = new List<Download>();
            try {
                string[] f = Directory.GetFiles(@$"{Configuration["DOC:BackupStoragePath"]}", "*.zip");
                foreach (string file in f) {
                    files.Add(new Download { FileName = Path.GetFileName(file) });
                }
                files = files.OrderByDescending(x => x.FileName).ToList();
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                Site.Messages.Enqueue(ex.Message);
            }
            return View(files);
        }

        // Stream (Post)
        [HttpGet, HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Stream(string id) {
            string path = @$"{Configuration["DOC:BackupStoragePath"]}\";
            string filePath = "";
            try {
                filePath = $@"{path}{id}";
                Response.Headers.Clear();
                Response.Headers.Add("Content-Disposition", @$"attachment;filename={id}");
                return PhysicalFile(filePath, MimeTypes.GetMimeType(filePath), Path.GetFileName(filePath));
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                Site.Messages.Enqueue(ex.Message);
            }
            return Json(false);
        }

        #endregion

    }

}

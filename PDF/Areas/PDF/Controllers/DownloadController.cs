#region Usings

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Serilog;
using PDF.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

#endregion

namespace PDF.Controllers {

    [Area("PDF")]
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
            List<Document> files = new();
            try {
                string[] f = Directory.GetFiles(@Configuration["PDFSettings:DownloadStoragePath"], "*.docx");
                foreach (string file in f) {
                    files.Add(new Document { FileName = Path.GetFileName(file) });
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
                Log.Logger.ForContext("UserId", User.UserId()).Information($"View PDF Converter Download Help");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return View();
        }

        // Stream (Post)
        [HttpGet, HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Stream([FromBody] IList<Document> documents) {
            string path = @Configuration["PDFSettings:DownloadStoragePath"];
            string filePath = "";
            try {
                foreach (Document b in documents) {
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

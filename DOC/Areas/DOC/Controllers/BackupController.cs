#region Usings

using DNA3.Models;
using DOC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Utilities;

#endregion

namespace SMO.Controllers {

    [Area("DOC")]
    [Authorize(Policy = "Administrators")]
    public class BackupController : Controller {

        #region Properties and Variables

        private readonly MainContext Context;
        private readonly ILogger<BackupController> Logger;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly IConfiguration Configuration;

        #endregion

        #region Class Methods

        // Constructor
        public BackupController(MainContext context, IConfiguration configuration, IWebHostEnvironment webhostenvironment, ILogger<BackupController> logger) {
            Context = context;
            Configuration = configuration;
            WebHostEnvironment = webhostenvironment;
            Logger = logger;
        }

        #endregion

        #region Controller Methods

        // Index (Get)
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index() {
            string message;
            string source = Path.Combine(WebHostEnvironment.WebRootPath, $"{Configuration["DOC:SourceFolderName"]}");
            string target = @$"{Configuration["DOC:BackupStoragePath"]}\{Strings.BackupFileName("Customers")}.zip";
            Backup instance = new Backup();
            try {
                instance.Directories = Directory.GetDirectories(source, "*", SearchOption.AllDirectories).Count();
                instance.Files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories).Count();
                instance.Uncompressed = Strings.BytesToString(new DirectoryInfo(source).EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length));
                if (System.IO.File.Exists(target)) {
                    instance.Compressed = Strings.BytesToString(new System.IO.FileInfo(target).Length);
                } else {
                    instance.Compressed = Strings.BytesToString(0);
                }
                Log.Logger.ForContext("UserId", User.UserId()).Information($"View Backup Summary");
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return View(instance);
        }

        // Compress (Get)
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Compress() {
            string message;
            string source = Path.Combine(WebHostEnvironment.WebRootPath, $"{Configuration["DOC: SourceFolderName"]}");
            string target = @$"{Configuration["DOC:BackupStoragePath"]}\{Strings.BackupFileName("Customers")}.zip";
            try {
                if (System.IO.File.Exists(target)) {
                    System.IO.File.Delete(target);
                }
                ZipFile.CreateFromDirectory(source, target);
                message = $"Created {target}";
                Site.Messages.Enqueue(message);
                Log.Logger.ForContext("UserId", User.UserId()).Information(message);
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return RedirectToAction("Index");
        }

        #endregion

    }

}

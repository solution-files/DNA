#region Usings

using CodeBeautify;
using DNA3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

    [Authorize(Policy = "Administrators")]
    public class ConfigController : Controller {

        #region Variables

        // Variables
        private readonly IConfiguration Configuration;
        private readonly ILogger<ConfigController> Logger;
        private readonly string Title = "Configuration";
        private string  ConfigFile = Path.Combine(AppContext.BaseDirectory, "appSettings.json");

        #endregion

        #region Methods

        // Constructor
        public ConfigController(IConfiguration configuration, ILogger<ConfigController> logger) {
            Configuration = configuration;
            Logger = logger;
        }

        #endregion

        #region Actions

        // Index (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Index(int? id) {
            AppSettings instance = new AppSettings();
            try {
                instance = JsonConvert.DeserializeObject<AppSettings>(System.IO.File.ReadAllText("appsettings.json"));
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title}");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return View("Detail", instance);
        }

        // Save (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Save(AppSettings instance) {
            string result;
            string message;
            try {
                if (ModelState.IsValid) {
                    result = JsonConvert.SerializeObject(instance, Formatting.Indented);
                    System.IO.File.WriteAllText("japsettings.json", result);
                    message = "Configuration file written to application folder";
                    Site.Messages.Enqueue(message);
                    Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                    return RedirectToAction("Index");
                } else {
                    var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                }
            } catch (Exception ex) {
                message = ex.Message;
                Logger.LogError(ex, message);
            }
            return View("Detail", instance);
        }

        // Close
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet, HttpPost]
        public IActionResult Close() {
            string message;
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Closed {Title}");
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return RedirectToAction("Index", "Dashboard");
        }

    }

    #endregion

}

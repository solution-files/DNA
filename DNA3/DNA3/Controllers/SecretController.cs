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
    public class SecretController : Controller {

        #region Variables

        // Variables
        private readonly IConfiguration Configuration;
        private readonly ILogger<SecretController> Logger;
        private readonly string Title = "Secret";
        private string ConfigFile;

        #endregion

        #region Methods

        // Constructor
        public SecretController(IConfiguration configuration, ILogger<SecretController> logger) {
            Configuration = configuration;
            Logger = logger;
            ConfigFile = Path.Combine("C:\\", "DNASettings.json");
        }

        #endregion

        #region Actions

        // Index (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Index(int? id) {
            Secret instance = new();
            try {
                Site.Messages.Enqueue("Secrets are encrypted using the Data Protector namespace and the storage location is specified in the Configuration Settings.");
                instance = JsonConvert.DeserializeObject<Secret>(System.IO.File.ReadAllText(ConfigFile));
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
        public IActionResult Save(Secret instance) {
            string result;
            string message;
            try {
                if (ModelState.IsValid) {
                    result = JsonConvert.SerializeObject(instance, Formatting.Indented);
                    System.IO.File.WriteAllText("hapsettings.json", result);
                    message = "Application secrets written to the specified location";
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

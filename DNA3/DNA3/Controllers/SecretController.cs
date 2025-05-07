#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Linq;
using Utilities;

#endregion

namespace DNA3.Controllers {

    [Authorize(Policy = "Administrators")]
    public class SecretController(IConfiguration configuration, ILogger<SecretController> logger, IHttpContextAccessor httpContextAccessor) : Controller {

        #region Variables

        // Variables
        private readonly IConfiguration Configuration = configuration;
        private readonly ILogger<SecretController> Logger = logger;
        private readonly IHttpContextAccessor HttpContextAccessor = httpContextAccessor;
        private readonly string Title = "Secret";
        private readonly string ConfigFile = Path.Combine("C:\\", "DNASettings.json");

        #endregion

        #region Controller Actions

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
                Logger.LogError(ex, "{message}", ex.Message);
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
                Logger.LogError(ex, "{message}", message);
            }
            return View("Detail", instance);
        }

        // Close
        [HttpGet, HttpPost]
        [Authorize(Policy = "Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Close() {
            string message;
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Closed {Title}");
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, "{message}", message);
            }
            return RedirectPermanent(HttpContextAccessor.HttpContext.Session.GetString("secretReturnUrl"));
        }

        // Close Index
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet, HttpPost]
        public IActionResult CloseIndex() {
            string message;
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Closed {Title}");
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, "{message}", message);
            }
            return RedirectToAction("Index", "Dashboard");
        }
    }

    #endregion

}

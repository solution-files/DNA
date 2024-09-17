#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

    [Authorize(Policy = "Administrators")]
    public class CertificateController : Controller {

        #region Variables

        // Variables
        private readonly IConfiguration Configuration;
        private readonly MainContext Context;
        private readonly ILogger<CertificateController> Logger;
        private readonly string Title = "Certificate";

        #endregion

        #region Class Methods

        // Constructor
        public CertificateController(IConfiguration configuration, MainContext context, ILogger<CertificateController> logger) {
            Configuration = configuration;
            Context = context;
            Logger = logger;
        }

        #endregion

        #region Controller Actions

        // New (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult New(Certificate instance) {
            try {
                if (instance == null) {
                    instance = new Certificate {

                    };
                }
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Initiate New {Title}");
            } catch (Exception ex) {
                string message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return View("Detail", instance);
        }

        // Save (Post)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Certificate instance) {
            try {
                if (ModelState.IsValid) {
                    Utilities.Security.CreateSelfSignedCertificate(instance.Name);
                    Site.Messages.Enqueue("Cretificate created sucessfully");
                }
            } catch (Exception ex) {
                string message = ex.Message;
                Logger.LogError(ex, message);
            }
            return View("Detail", instance);
        }

        // Close
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
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

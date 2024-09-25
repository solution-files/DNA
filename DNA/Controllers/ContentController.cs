#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Mvc;
using Utilities;

#endregion

namespace DNA.Controllers {

    public class ContentController : Controller {

        #region Variables

        private readonly IConfiguration Configuration;
        private readonly MainContext Context;
        private readonly ILogger<ContentController> Logger;

        #endregion

        #region Methods

        public ContentController(IConfiguration configuration, MainContext context, ILogger<ContentController> logger) {
            Configuration = configuration;
            Context = context;
            Logger = logger;
        }

        #endregion

        #region Controller Actions

        // Index (Get)
        public IActionResult Index() {
            string message;
            try {
                // Placeholder
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return View();
        }

        #endregion

    }

}

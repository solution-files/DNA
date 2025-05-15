#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Mvc;
using Utilities;

#endregion

namespace DNA.Controllers {

    public class ContentController(IConfiguration configuration, MainContext context, ILogger<ContentController> logger) : Controller {

        #region Variables

        private readonly IConfiguration Configuration = configuration;
        private readonly MainContext Context = context;
        private readonly ILogger<ContentController> Logger = logger;

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
                Logger.LogError(ex, "{message}", message);
            }
            return View();
        }

        #endregion

    }

}

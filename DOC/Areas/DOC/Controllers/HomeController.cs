#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Utilities;

#endregion

namespace DOC.Controllers {

    [Area("DOC")]
    public class HomeController : Controller {

        #region Properties and Variables

        private readonly MainContext Context;
        private readonly ILogger<HomeController> Logger;

        #endregion

        #region Class Methods

        // Constructor
        public HomeController(MainContext context, ILogger<HomeController> logger) {
            Context = context;
            Logger = logger;
        }

        #endregion

        #region Controller Actions

        // Index (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Index() {
            Site.Messages.Enqueue("Introducing SQL Server for Linux. Add a Standard, Developer, or Express edition to your server today!");
            return View();
        }

        #endregion

    }

}

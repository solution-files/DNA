#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Utilities;

#endregion

namespace Schedule.Controllers {

    #region Class

    [Area("Schedule")]
    public class HomeController(MainContext context, ILogger<HomeController> logger) : Controller {

        #region Variables

        private readonly MainContext Context = context;
        private readonly ILogger<HomeController> Logger = logger;

        #endregion

        #region Controller Actions

        // Index (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        #endregion

    }

    #endregion

}

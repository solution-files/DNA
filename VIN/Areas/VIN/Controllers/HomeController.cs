#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Utilities;

#endregion

namespace VIN.Controllers {

    #region Class

    [Area("VIN")]
    [Authorize(Policy = "Administrators")]
    public class HomeController : Controller {

		#region Variables

		private readonly MainContext Context;
		private readonly ILogger<HomeController> Logger;

		#endregion

		#region Methods

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
            return View();
        }

		#endregion

	}

    #endregion

}

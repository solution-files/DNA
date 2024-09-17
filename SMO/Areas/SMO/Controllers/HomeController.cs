#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Utilities;

#endregion

namespace SMO.Controllers {

    #region Class

    [Area("SMO")]
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

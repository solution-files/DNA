#region Usings

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDF.Models;
using System.Diagnostics;

#endregion

namespace PDF.Controllers {

    [Area("PDF")]
    [Authorize(Policy = "Users")]
    public class HomeController : Controller {

        #region Properties

        private readonly ILogger<HomeController> _logger;

        #endregion

        #region Class Methods

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        #endregion

        #region Controller Actions

        public IActionResult Index() {
            return View();
        }

        #endregion

    }

}

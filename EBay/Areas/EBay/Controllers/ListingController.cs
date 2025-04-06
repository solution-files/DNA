#region Usings

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

#endregion

namespace EBay.Controllers {

    #region Class

    [Area("EBay")]
    [Authorize(Policy = "Users")]
    public class ListingController : Controller {

        #region Variables

        private readonly ILogger<ListingController> Logger;

        #endregion

        #region Methods

        // Constructor
        public ListingController(ILogger<ListingController> logger) {
            Logger = logger;
        }

        #endregion

        #region Controller Actions

        [HttpGet]
        public IActionResult Index() {
            try {

            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Active() {
            try {

            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Complete() {
            try {

            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Copy() {
            try {

            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Estimate() {
            try {

            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Search() {
            try {

            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Watch() {
            try {

            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return View();
        }

        #endregion

    }

    #endregion

}

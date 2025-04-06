#region Usings

using DNA3.Models;
using EBay.Classes;
using EBay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

#endregion

namespace EBay.Controllers {

    #region Class

    [Area("EBay")]
    [Authorize(Policy = "Users")]
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
        public async Task<IActionResult> Index() {
            string token = HttpContext.Session.GetString("EbayUserAccessToken");
            if (string.IsNullOrEmpty(token)) {
                string code = Request.Query["code"];
                if (string.IsNullOrEmpty(code)) {
                    string authUrl = $"https://auth.ebay.com/oauth2/authorize?client_id=HostingX-e2bb-4a7b-ae2d-849dc38897f2&redirect_uri={Uri.EscapeDataString("https://dotnetadmin.com/ebay")}&response_type=code&scope={Uri.EscapeDataString("https://api.ebay.com/oauth/api_scope")}";
                    return Redirect(authUrl);
                } else {
                    token = await OAuthHelper.CreateUserAccessToken(code);
                    HttpContext.Session.SetString("EbayUserAccessToken", token);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        // Authenticate (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Authenticate() {
            Authenticate model = new(); ;
            try {
                model.UserToken = HttpContext.Session.GetString("EbayUserAccessToken");
            } catch(Exception ex) {
                Common.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return View(model);
        }

        // Save (Post)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public IActionResult Save(Authenticate model) {
            try {
                HttpContext.Session.SetString("EbayUserAccessToken", model.UserToken);
            } catch (Exception ex) {
                Common.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return View("Authenticate", model);
        }

        #endregion

    }

    #endregion

}

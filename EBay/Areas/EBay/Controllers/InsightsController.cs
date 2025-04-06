#region Usings

using DNA3.Classes;
using DNA3.Models;
using EBay.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

#endregion

namespace EBay.Controllers {

    #region Class

    [Area("EBay")]
    [Authorize(Policy = "Users")]
    public class InsightsController : Controller {

        #region Variables

        private readonly MainContext Context;
        private readonly ILogger<InsightsController> Logger;

        #endregion

        #region Methods

        // Constructor
        public InsightsController(MainContext context, ILogger<InsightsController> logger) {
            Context = context;
            Logger = logger;
        }

        #endregion

        #region Controller Actions

        // Index Get
        [HttpGet]
        public async Task<IActionResult> Index() {
            try {
                EBay.Models.Insights.Root instance = new();
                instance.itemSales = new();
                ViewBag.ConditionList = await Context.Condition.OrderBy(x => x.Name).ToListAsync();
                ViewBag.SortOrderList = await Context.SortOrder.OrderBy(x => x.Name).ToListAsync();
                ViewBag.ListingTypeList = await Context.ListingType.OrderBy(x => x.Name).ToListAsync();
                return View(instance);

            } catch (Exception ex) {
                Common.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }

        // Index (Post)
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> IndexAsync(string keywords) {
            EBay.Models.Insights.Root result;
            try {
                string token = HttpContext.Session.GetString("EbayApplicationToken");
                if (string.IsNullOrEmpty(token)) {
                    token = EBay.Classes.OAuthHelper.CreateApplicationToken();
                    HttpContext.Session.SetString("EbayApplicationToken", token);
                }
                using (HttpClient client = new()) {
                    client.BaseAddress = new Uri("https://api.ebay.com/buy/marketplace_insights/v1_beta/item_sales/");
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                    client.DefaultRequestHeaders.Add("X-EBAY-C-MARKETPLACE-ID", "EBAY_US");
                    HttpResponseMessage responseMessage = await client.GetAsync("search?q={" + keywords + "}&limit=30&filter=buyingOptions:{AUCTION | FIXED_PRICE}&fieldgroups=EXTENDED");
                    if (responseMessage.IsSuccessStatusCode) {
                        result = JsonConvert.DeserializeObject<EBay.Models.Insights.Root>(responseMessage.Content.ReadAsStringAsync().Result);
                    } else {
                        result = default;
                    }
                }
            } catch (Exception ex) {
                result = default;
                Logger.LogError(ex.Message);
            }
            return View(result);
        }

        // Close
        [HttpGet, HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Close() {
            return RedirectToAction("Home", "Index");
        }

        #endregion

    }

    #endregion

}

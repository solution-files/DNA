#region Usings

using EBay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

#endregion

namespace EBay.Controllers {

    #region Class

    [Area("EBay")]
    [Authorize(Policy = "Administrators")]
    public class InventoryController : Controller {

        #region Variables

        private readonly ILogger<InventoryController> Logger;

        #endregion

        #region Methods

        // Constructor
        public InventoryController(ILogger<InventoryController> logger) {
            Logger = logger;
        }

        #endregion

        #region Controller Actions

        // Index (Get)
        [HttpGet]
        public async Task<IActionResult> Index() {
            Inventory result;
            try {
                string token = HttpContext.Session.GetString("EbayUserAccessToken");
                using (HttpClient client = new()) {
                    client.BaseAddress = new Uri("https://api.ebay.com/sell/inventory/v1/");
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                    client.DefaultRequestHeaders.Add("X-EBAY-C-MARKETPLACE-ID", "EBAY_US");
                    HttpResponseMessage responseMessage = await client.GetAsync("inventory_item?limit=50&offset=0");
                    if (responseMessage.IsSuccessStatusCode) {
                        result = JsonConvert.DeserializeObject<Inventory>(responseMessage.Content.ReadAsStringAsync().Result);
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

        #endregion

    }

    #endregion

}

#region Usings

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VIN.Models;

#endregion

namespace VIN.Controllers {

    #region Class

    [Area("VIN")]
    [Authorize(Policy = "Administrators")]
    public class SearchController : Controller {

        #region Variables

        private readonly IConfiguration Configuration;
        private readonly ILogger<SearchController> Logger;
        private readonly string? ApiKey;

        #endregion

        #region Methods

        // Constructor
        public SearchController(IConfiguration configuration, ILogger<SearchController> logger) {
            Configuration = configuration;
            Logger = logger;

            try {
                ApiKey = Configuration["Auto:ApiKey"];
                if (string.IsNullOrEmpty(ApiKey)) {
                    throw new ArgumentException("The Auto.dev Api Key must be defined and accessible to the Configuration Manager");
                } else {
                    ApiKey = Configuration["Auto:ApiKey"];
                }
            } catch (Exception ex) {
                Utilities.Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
        }

        #endregion

        #region Controller Actions

        // Index (Get)
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Index() {
            Root? result = default;
            try {
                using (HttpClient client = new()) {
                    client.BaseAddress = new Uri("https://auto.dev/api/listings");
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
                    HttpResponseMessage responseMessage = await client.GetAsync("?year_min=2017");
                    if (responseMessage.IsSuccessStatusCode) {
                        result = JsonConvert.DeserializeObject<Root>(responseMessage.Content.ReadAsStringAsync().Result);
                    }
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return View(result.records);
        }

        #endregion

    }

    #endregion

}

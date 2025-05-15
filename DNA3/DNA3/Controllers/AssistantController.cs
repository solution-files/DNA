#region Usings

using DNA3.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

    [Authorize(Policy = "Administrators")]
    public class AssistantController : Controller {

        #region Variables

        // Variables
        private readonly IConfiguration Configuration;
        private readonly MainContext Context;
        private readonly IHttpClientFactory HttpClientFactory;
        private readonly ILogger<AssistantController> Logger;
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly string Title = "Assistant";
        private readonly string ApiKey;

        #endregion

        #region Constructor

        public AssistantController(IConfiguration configuration, MainContext context, IHttpClientFactory httpclientfactory, ILogger<AssistantController> logger, IHttpContextAccessor httpContextAccessor) {
            Configuration = configuration;
            Context = context;
            HttpClientFactory = httpclientfactory;
            Logger = logger;
            HttpContextAccessor = httpContextAccessor;
            ApiKey = Configuration["Gemini:APIKey"];
        }

        #endregion

        #region Controller Actions

        // Index (Get)
        [HttpGet]
        public IActionResult Index(string criteria) {
            HttpContextAccessor.HttpContext.Session.SetString("assistantReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
            if (!string.IsNullOrEmpty(criteria)) {
                Site.Messages.Enqueue(criteria);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Index(DNA3.Models.Gemini instance) {
            string result;
            string message;
            StringContent content;
            try {
                using HttpClient client = new();
                client.BaseAddress = new Uri(Configuration["Gemini:APIEndPoint"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-goog-api-key", Configuration["Gemini:APIKey"]);
                var JsonPayload = JsonConvert.SerializeObject(instance);
                content = new StringContent(JsonPayload, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json"));
                HttpResponseMessage response = await client.PostAsync(client.BaseAddress.ToString(), content);
                if (response.IsSuccessStatusCode) {
                    result = await response.Content.ReadAsStringAsync();
                } else {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new ArgumentException($"Error posting form data: {response.StatusCode}, {errorContent}");
                }
            } catch(Exception ex) {
                message = ex.Message;
                result = message;
                Logger.LogError(ex, "{message}", message);
            }
            return View(result);
        }

        // Close
        [HttpGet, HttpPost]
        [Authorize(Policy = "Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Close() {
            string message;
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Closed {Title}");
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, "{message}", message);
            }
            return RedirectPermanent(HttpContextAccessor.HttpContext.Session.GetString("assistantReturnUrl"));
        }

        // Close Index
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet, HttpPost]
        public IActionResult CloseIndex() {
            string message;
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Closed {Title}");
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, "{message}", message);
            }
            return RedirectToAction("Index", "Dashboard");
        }

    }

    #endregion

}

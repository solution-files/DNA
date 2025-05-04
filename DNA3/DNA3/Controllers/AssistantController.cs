#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private IHttpClientFactory HttpClientFactory;
        private readonly ILogger<AssistantController> Logger;
        private readonly string Title = "Assistant";
        private string ApiKey;

        #endregion

        #region Constructor

        public AssistantController(IConfiguration configuration, MainContext context, IHttpClientFactory httpclientfactory, ILogger<AssistantController> logger) {
            Configuration = configuration;
            Context = context;
            HttpClientFactory = httpclientfactory;
            Logger = logger;
            ApiKey = Configuration["Gemini:APIKey"];
        }

        #endregion

        #region Controller Actions

        // Index (Get)
        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public async Task<IActionResult> GetGeminiResponse([FromBody] object obj) {
            if (obj is null) {
                return BadRequest("Prompt cannot be empty.");
            }

            using (HttpClient httpClient = HttpClientFactory.CreateClient()) {
                httpClient.DefaultRequestHeaders.Add("x-goog-api-key", ApiKey);

                var requestBody = new {
                    contents = new[] { new { parts = new[] { new { text = Utilities.Strings.GetJsonProperty(obj, "prompt") } } } }
                };

                var jsonRequestBody = System.Text.Json.JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                try {
                    var response = await httpClient.PostAsync("https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent", content);
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var geminiResponse = System.Text.Json.JsonSerializer.Deserialize<GeminiResponse>(responseContent);

                    if (geminiResponse?.candidates?.FirstOrDefault()?.content?.parts?.FirstOrDefault()?.text != null) {
                        return Ok(geminiResponse.candidates.First().content.parts.First().text);
                    } else {
                        return StatusCode(500, "Failed to extract response from Gemini.");
                    }
                } catch (HttpRequestException ex) {
                    return StatusCode(500, $"Error communicating with Gemini: {ex.Message}");
                }
            }
        }

        // Close
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Close() {
            string message;
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Closed {Title}");
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return RedirectToAction("Index", "Dashboard");
        }

    }

    #endregion

}

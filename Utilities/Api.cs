#region Usings

using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

#endregion

namespace Utilities {

    public class Api {

        #region Async Methods

        // Response
        [HttpGet, Produces("text/plain")]
        public static async Task<string> ResponseAsync(string baseaddress, string method) {
            string result;
            using (HttpClient client = new()) {
                client.BaseAddress = new Uri(baseaddress);
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage responseMessage = await client.GetAsync(method);
                if (responseMessage.IsSuccessStatusCode) {
                    result = responseMessage.Content.ReadAsStringAsync().Result;
                } else {
                    result = responseMessage.ReasonPhrase;
                }
            }
            return result;
        }

        #endregion

    }
}

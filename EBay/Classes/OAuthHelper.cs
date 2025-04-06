#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace EBay.Classes {

    #region Class

    public class OAuthHelper {

        #region Variables

        private static readonly string oauthRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../ebay_oauth/");
        private static readonly string oauthApplicationTokenFile = Path.Combine(oauthRoot, "ebay_application_token.txt");
        private static readonly string oauthUserAccessTokenFile = Path.Combine(oauthRoot, "ebay_user_access_token.txt");
        private static readonly string redirectUri = "https://dotnetadmin.com/ebay";
        private static readonly string scope = "https://api.ebay.com/oauth/api_scope";
        private static readonly string clientId = "HostingX-e2bb-4a7b-ae2d-849dc38897f2";
        private static readonly string clientSecret = "9e95a50b-2337-4d92-8735-812e076a7053";
        private static readonly string tokenEndpoint = "https://api.ebay.com/identity/v1/oauth2/token";
        private static readonly string eBayAuthUrl = "https://auth.ebay.com/oauth2/authorize";

        #endregion

        #region Methods

        // Create User Access Token
        public static async Task<string> CreateUserAccessToken(string authorizationCode) {
            string token = "";
            try {
                using (HttpClient client = new()) {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));
                    var content = new FormUrlEncodedContent(new Dictionary<string, string> {{ "grant_type", "authorization_code" }, { "code", authorizationCode }, { "redirect_uri", redirectUri }});
                    HttpResponseMessage response = await client.PostAsync(tokenEndpoint, content);
                    if (response.IsSuccessStatusCode) {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        dynamic tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                        token = tokenResponse?.access_token;
                    }
                }
            } catch(Exception ex) {
                token = ex.Message;
            }
            return token;
        }

        // Get User Access Token
        public static string GetUserAccessToken() {
            if (File.Exists(oauthUserAccessTokenFile)) {
                DateTime lastWriteTime = File.GetLastWriteTime(oauthUserAccessTokenFile);
                DateTime now = DateTime.Now;
                TimeSpan duration = TimeSpan.FromSeconds(86400); // 24 hours
                int margin = 30; // remaining seconds before we request a new token

                if (lastWriteTime.Add(duration).AddSeconds(-margin) > now) {
                    return File.ReadAllText(oauthUserAccessTokenFile);
                } else {
                    return "User is not logged in to EBay";
                }
            } else {
                return "User is not logged in to EBay";
            }
        }


        // Create Application Token
        public static string CreateApplicationToken() {
            string url = "https://api.ebay.com/identity/v1/oauth2/token";
            string authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            string body = $"grant_type=client_credentials&scope=https://api.ebay.com/oauth/api_scope";

            try {
                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";
                request.Headers.Add("Authorization", "Basic " + authorization);
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] data = Encoding.UTF8.GetBytes(body);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream()) {
                    stream.Write(data, 0, data.Length);
                }

                using (WebResponse response = request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new(responseStream)) {
                    string jsonResponse = reader.ReadToEnd();
                    dynamic token = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);
                    if (token.access_token != null) {
                        //File.WriteAllText(oauthApplicationTokenFile, token.access_token);
                        return token.access_token;
                    } else {
                        return "ERR: could not access token";
                    }
                }
            } catch (WebException ex) {
                return "ERR: " + ex.Message;
            }
        }

        // Get Application Token
        public static string GetApplicationToken() {
            if (File.Exists(oauthApplicationTokenFile)) {
                DateTime lastWriteTime = File.GetLastWriteTime(oauthApplicationTokenFile);
                DateTime now = DateTime.Now;
                TimeSpan duration = TimeSpan.FromSeconds(86400); // 24 hours
                int margin = 30; // remaining seconds before we request a new token

                if (lastWriteTime.Add(duration).AddSeconds(-margin) > now) {
                    return File.ReadAllText(oauthApplicationTokenFile);
                } else {
                    return CreateApplicationToken();
                }
            } else {
                return CreateApplicationToken();
            }
        }

        #endregion

    }

    #endregion

}
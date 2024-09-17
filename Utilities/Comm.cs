#region Usings

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;
using Utilities.Models;

#endregion

namespace Utilities {

    public class Comm : BackgroundService {

        #region Variables

        private readonly ILogger<Comm> Logger;
        private HttpResponseMessage Response;

        #endregion

        #region Methods

        public Comm(ILogger<Comm> logger) {
            Logger = logger;
        }

        #endregion

        #region Async Methods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            try {
                Device device = new Device {
                    Hostname = Environment.MachineName,
                    Ipv4 = Utilities.Network.GetLocalIpAddress()
                };

                string endpoint = "https://admin.clicktickdone.com/api/device";
                Logger.LogInformation($"Endpoint set to: {endpoint}");

                while (!stoppingToken.IsCancellationRequested) {

                    if (Network.InternetActive()) {

                        HttpClientHandler handler = new HttpClientHandler {
                            ClientCertificateOptions = ClientCertificateOption.Manual,
                            SslProtocols = SslProtocols.Tls12
                        };
                        handler.ClientCertificates.Add(new X509Certificate2("CTD.pfx"));

                        using (HttpClient client = new HttpClient(handler)) {
                            StringContent content = new StringContent(JsonConvert.SerializeObject(device), Encoding.UTF8, "application/json");
                            Response = await client.PostAsync(endpoint, content, stoppingToken);
                            if (Response.StatusCode == HttpStatusCode.OK) {
                                Logger.LogInformation($"Connected via {device.Ipv4}");
                            } else {
                                Logger.LogWarning("Connection failed");
                            }
                        }
                    }

                    // Wait 60 seconds and repeat until stopped
                    await Task.Delay(10000, stoppingToken);
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

    }

}

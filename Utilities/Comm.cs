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

    public class Comm(ILogger<Comm> logger) : BackgroundService {

        #region Variables
        private HttpResponseMessage Response;

        #endregion

        #region Async Methods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            try {
                Device device = new() {
                    Hostname = Environment.MachineName,
                    Ipv4 = Utilities.Network.GetLocalIpAddress()
                };

                string endpoint = "https://admin.clicktickdone.com/api/device";
                logger.LogInformation("Endpoint set to: {endpoint}", endpoint);

                while (!stoppingToken.IsCancellationRequested) {

                    if (Network.InternetActive()) {

                        HttpClientHandler handler = new() {
                            ClientCertificateOptions = ClientCertificateOption.Manual,
                            SslProtocols = SslProtocols.Tls12
                        };
                        handler.ClientCertificates.Add(new X509Certificate2("CTD.pfx"));

                        using HttpClient client = new(handler);
                        StringContent content = new(JsonConvert.SerializeObject(device), Encoding.UTF8, "application/json");
                        Response = await client.PostAsync(endpoint, content, stoppingToken);
                        if (Response.StatusCode == HttpStatusCode.OK) {
                            logger.LogInformation("Connected via {device}", device.Ipv4);
                        } else {
                            logger.LogWarning("Connection failed");
                        }
                    }

                    // Wait 60 seconds and repeat until stopped
                    await Task.Delay(10000, stoppingToken);
                }
            } catch (Exception ex) {
                logger.LogError(ex, "{message}", ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

    }

}

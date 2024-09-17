#region Usings

using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Utilities {

    public class Network {

        #region Connection Information

        // Email Address Exists (TODO: SSL Port, Lookup SMTP Server from MX Record)
        public static bool EmailAddressExists(string EmailAddress, string SmtpServer = "gmail-smtp-in.l.google.com") {
            bool ReturnValue;
            try {
                TcpClient tClient = new TcpClient(SmtpServer, 25);
                string CRLF = "\r\n";
                byte[] dataBuffer;
                string ResponseString;
                NetworkStream netStream = tClient.GetStream();
                StreamReader reader = new StreamReader(netStream);
                ResponseString = reader.ReadLine();
                dataBuffer = Strings.BytesFromString("HELO KirtanHere" + CRLF);
                netStream.Write(dataBuffer, 0, dataBuffer.Length);
                ResponseString = reader.ReadLine();
                dataBuffer = Strings.BytesFromString("MAIL FROM:<YourGmailIDHere@gmail.com>" + CRLF);
                netStream.Write(dataBuffer, 0, dataBuffer.Length);
                ResponseString = reader.ReadLine();
                dataBuffer = Strings.BytesFromString("RCPT TO:<" + EmailAddress.Trim() + ">" + CRLF);
                netStream.Write(dataBuffer, 0, dataBuffer.Length);
                ResponseString = reader.ReadLine();
                if (int.Parse(ResponseString.Substring(0, 3)) == 550) {
                    ReturnValue = false;
                } else {
                    ReturnValue = true;
                }
                dataBuffer = Strings.BytesFromString("QUITE" + CRLF);
                netStream.Write(dataBuffer, 0, dataBuffer.Length);
                tClient.Close();
            } catch {
                ReturnValue = false;
            }
            return ReturnValue;
        }

        // Internet Active
        public static bool InternetActive(int timeoutMs = 3000, string url = null) {
            try {
                if (url == null) {
                    url = "http://mit.edu";
                }
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = timeoutMs;
                using (var response = (HttpWebResponse)request.GetResponse())
                    return true;
            } catch {
                return false;
            }
        }

        // Get Hostname
        public static string GetHostname() {
            string result;
            try {
                if (InternetActive()) {
                    result = Dns.GetHostName();
                } else {
                    result = "localhost";
                }
            } catch(Exception ex) {
                Log.Error(ex, ex.Message);
                result = "error";
            }
            return result;
        }

        // Get Local IP Address
        public static string GetLocalIpAddress() {
            string result = "";
            try {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
                    socket.Connect("admin.clicktickdone.com", 1024);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    result = endPoint.Address.ToString();
                }
            } catch (Exception ex) {
                Log.Error(ex, ex.Message);
                result = ex.Message;
            }
            return result;
        }

        //public static string GetLocalIpAddress(NetworkInterfaceType interfacetype = NetworkInterfaceType.Wireless80211) {
        //    string returnvalue = null;
        //    try {
        //        returnvalue = NetworkInterface
        //            .GetAllNetworkInterfaces()
        //            .Where(n => n.OperationalStatus == OperationalStatus.Up)
        //            .Where(n => n.NetworkInterfaceType == interfacetype)
        //            .SelectMany(n => n.GetIPProperties()?.UnicastAddresses)
        //            .Where(n => n.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        //            .Select(g => g?.Address)
        //            .Where(a => a != null)
        //            .FirstOrDefault().ToString();
        //    } catch {
        //        returnvalue = null;
        //    }
        //    return returnvalue.ToString();
        //}

        // Get Default Gateway
        public static System.Net.IPAddress GetDefaultGateway() {
            IPAddress returnvalue = null;
            try {
                returnvalue = NetworkInterface
                    .GetAllNetworkInterfaces()
                    .Where(n => n.OperationalStatus == OperationalStatus.Up)
                    .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                    .Select(g => g?.Address)
                    .Where(a => a != null)
                    .FirstOrDefault();
            } catch {
                returnvalue = null;
            }
            return returnvalue;
        }

        #endregion

    }

}

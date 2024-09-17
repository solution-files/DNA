#region Usings

using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Xml.Linq;

#endregion

namespace Utilities {

    public class AddressApi {

        #region Methods

        // Standardize Mailing Address
        public static MailingAddress StandardizeMailingAddress(MailingAddress request) {
            MailingAddress result = new();
            try {

                XDocument requestDoc = new(
                    new XElement("AddressValidateRequest",
                    new XAttribute("USERID", "45768CLICKQ65"),
                    new XElement("Revision", "1"),
                    new XElement("Address",
                        new XAttribute("ID", "0"),
                        new XElement("Address1", request.Address),
                        new XElement("Address2", request.Address),
                        new XElement("City", request.City),
                        new XElement("State", request.State),
                        new XElement("Zip5", request.Zip5),
                        new XElement("Zip4", request.Zip4)
                        )
                    )
                );

                var url = "https://secure.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + requestDoc;
                var client = new WebClient();
                string response = client.DownloadString(url);
                XDocument xdoc = XDocument.Parse(response.ToString());
                Console.WriteLine(xdoc.ToString());
                foreach (XElement element in xdoc.Descendants("Address")) {
                    result.Id = Convert.ToInt32(XML.GetXMLAttribute(element, "ID"));
                    result.Address = XML.GetXMLElement(element, "Address1");
                    result.Address = XML.GetXMLElement(element, "Address2");
                    result.City = XML.GetXMLElement(element, "City");
                    result.County = XML.GetXMLElement(element, "County");
                    result.State = XML.GetXMLElement(element, "State");
                    result.Zip5 = XML.GetXMLElement(element, "Zip5");
                    result.Zip4 = XML.GetXMLElement(element, "Zip4");
                }

            } catch {
                // Do nothing for now
            }
            return result;
        }

        // Standardize
        public static USPSAddress Standardize(USPSAddress request) {
            USPSAddress result = new();
            try {

                XDocument requestDoc = new(
                    new XElement("AddressValidateRequest",
                    new XAttribute("USERID", "45768CLICKQ65"),
                    new XElement("Revision", "1"),
                    new XElement("Address",
                        new XAttribute("ID", "0"),
                        new XElement("Address1", request.Address1),
                        new XElement("Address2", request.Address2),
                        new XElement("City", request.City),
                        new XElement("State", request.State),
                        new XElement("Zip5", request.Zip5),
                        new XElement("Zip4", request.Zip4)
                        )
                    )
                );

                var url = "https://secure.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + requestDoc;
                var client = new WebClient();
                string response = client.DownloadString(url);
                XDocument xdoc = XDocument.Parse(response.ToString());
                Console.WriteLine(xdoc.ToString());
                foreach (XElement element in xdoc.Descendants("Address")) {
                    result.Id = Convert.ToInt32(XML.GetXMLAttribute(element, "ID"));
                    result.Address1 = XML.GetXMLElement(element, "Address1");
                    result.Address2 = XML.GetXMLElement(element, "Address2");
                    result.City = XML.GetXMLElement(element, "City");
                    result.County = XML.GetXMLElement(element, "County");
                    result.State = XML.GetXMLElement(element, "State");
                    result.Zip5 = XML.GetXMLElement(element, "Zip5");
                    result.Zip4 = XML.GetXMLElement(element, "Zip4");
                }

            } catch {
                // Do nothing for now
            }
            return result;
        }

        #endregion

    }

}
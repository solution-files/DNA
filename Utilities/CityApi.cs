#region Usings

using System;
using System.Net;
using System.Xml.Linq;

#endregion

namespace Utilities {

    public class CityApi {

        #region Methods

        public static void Main() {
            XDocument requestDoc = new XDocument(
                new XElement("CityStateLookupRequest",
                    new XAttribute("USERID", ""),
                    new XElement("Revision", "1"),
                    new XElement("ZipCode",
                        new XAttribute("ID", "0"),
                        new XElement("Zip5", "84414")
                    ),
                    new XElement("ZipCode",
                        new XAttribute("ID", "1"),
                        new XElement("Zip5", "84660")
                    )
                )
            );

            try {
                var url = "http://production.shippingapis.com/ShippingAPI.dll?API=CityStateLookup&XML=" + requestDoc;
                Console.WriteLine(url);
                var client = new WebClient();
                var response = client.DownloadString(url);

                var xdoc = XDocument.Parse(response.ToString());
                Console.WriteLine(xdoc.ToString());

                foreach (XElement element in xdoc.Descendants("ZipCode")) {
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine("ZipCode ID:	" + XML.GetXMLAttribute(element, "ID"));
                    Console.WriteLine("City:		" + XML.GetXMLElement(element, "City"));
                    Console.WriteLine("State:		" + XML.GetXMLElement(element, "State"));
                    Console.WriteLine("Zip5:		" + XML.GetXMLElement(element, "Zip5"));
                }
            } catch (WebException e) {
                Console.WriteLine(e.ToString());
            }
        }

    }

    #endregion

}

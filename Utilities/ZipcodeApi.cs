#region Usings

using System;
using System.Net;
using System.Xml.Linq;

#endregion

namespace Utilities {

    public class ZipcodeApi {

        #region Methods

        public static void Main() {
            XDocument requestDoc = new XDocument(
                new XElement("ZipCodeLookupRequest",
                    new XAttribute("USERID", ""),
                    new XElement("Revision", "1"),
                    new XElement("Address",
                        new XAttribute("ID", "0"),
                        new XElement("Address1", "2335 S State"),
                        new XElement("Address2", "Suite 300"),
                        new XElement("City", "Provo"),
                        new XElement("State", "UT"),
                        new XElement("Zip5", "84604"),
                        new XElement("Zip4", "")
                    ),
                    new XElement("Address",
                        new XAttribute("ID", "1"),
                        new XElement("Address1", "1427 S 2340 E"),
                        new XElement("Address2", ""),
                        new XElement("City", "Spanish Fork"),
                        new XElement("State", "UT"),
                        new XElement("Zip5", "84660"),
                        new XElement("Zip4", "")
                    )
                )
            );

            try {
                var url = "http://production.shippingapis.com/ShippingAPI.dll?API=ZipCodeLookup&XML=" + requestDoc;
                Console.WriteLine(url);
                var client = new WebClient();
                var response = client.DownloadString(url);

                var xdoc = XDocument.Parse(response.ToString());
                Console.WriteLine(xdoc.ToString());

                foreach (XElement element in xdoc.Descendants("Address")) {
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine("Address ID:		" + XML.GetXMLAttribute(element, "ID"));
                    Console.WriteLine("Address1:		" + XML.GetXMLElement(element, "Address1"));
                    Console.WriteLine("Address2:		" + XML.GetXMLElement(element, "Address2"));
                    Console.WriteLine("City:			" + XML.GetXMLElement(element, "City"));
                    Console.WriteLine("State:			" + XML.GetXMLElement(element, "State"));
                    Console.WriteLine("Zip5:			" + XML.GetXMLElement(element, "Zip5"));
                    Console.WriteLine("Zip4:			" + XML.GetXMLElement(element, "Zip4"));
                    Console.WriteLine("Urbanization:	" + XML.GetXMLElement(element, "Urbanization"));
                }
            } catch (WebException e) {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion

    }

}
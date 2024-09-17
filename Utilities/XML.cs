#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

#endregion

namespace Utilities {

    public class XML {

        #region Methods

        // Get XML Element
        public static string GetXMLElement(XElement element, string name) {
            var el = element.Element(name);
            if (el != null) {
                return el.Value;
            }
            return "";
        }

        // Get XML Attribute
        public static string GetXMLAttribute(XElement element, string name) {
            var el = element.Attribute(name);
            if (el != null) {
                return el.Value;
            }
            return "";
        }

        #endregion

    }

}

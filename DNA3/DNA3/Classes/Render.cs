using Microsoft.AspNetCore.Html;
using System;
using Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNA3.Classes {
    public class Render {

        /// <summary>
        /// Render Content - Parse HTML, render short codes, and return raw HTML
        /// </summary>
        /// <param name="value">Raw HTML content object</param>
        /// <returns>Raw HTML content with fully rendered short codes</returns>
        public static IHtmlContent Content(object value) {
            IHtmlContent result = null;
            try {
                result = new HtmlString(value.ToString());
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
            }
            return result;
        }

    }

}

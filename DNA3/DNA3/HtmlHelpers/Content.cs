
#region Usings

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

#endregion

namespace DNA3.HtmlHelpers {

	public static class Content {

		// Render - Return an unencoded string with appropriate subsitution of Shortcodes and Widgets. (In progress)
		public static HtmlString Render(this IHtmlHelper helper, string text, int length = 0) {
            if (length == 0) {
                return new HtmlString(text);
            } else {
                return new HtmlString(text.Substring(0, length));
            }
        }

	}

}
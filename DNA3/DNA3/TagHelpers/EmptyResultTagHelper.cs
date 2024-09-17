#region Usings

using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

#endregion

namespace DNA3.TagHelpers {

	[HtmlTargetElement("emptyresult")]
	public class EmptyResultTagHelper : TagHelper {

		#region Properties and Variables

		public string icon { get; set; }
		public string message { get; set; }

		#endregion

		#region Class Methods

		public EmptyResultTagHelper() {
			if (string.IsNullOrEmpty(message)) {
				message = "No entries match your selection criteria";
			}
		}

		public override void Process(TagHelperContext context, TagHelperOutput output) {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<div class='row'>");
			sb.AppendLine("<div class='col d-flex flex-column justify-content-center align-items-center'>");
			sb.AppendLine($"<i class='{icon} fa-10x my-3'></i>");
			sb.AppendLine($"<span class='text-muted mb-3'>{message}</span>");
			sb.AppendLine("</div>");
			sb.AppendLine("</div>");
			output.TagName = "EmptyResult";
			output.TagMode = TagMode.StartTagAndEndTag;
			output.PreContent.SetHtmlContent(sb.ToString());
		}

		#endregion

	}

}

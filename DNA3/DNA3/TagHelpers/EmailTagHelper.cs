#region Usings

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

#endregion

namespace DNA3.TagHelpers {

    [HtmlTargetElement("email")]
    public class EmailTagHelper(IConfiguration configuration) : TagHelper {

        #region Properties and Variables

        public string to { get; set; }

        private readonly IConfiguration Configuration = configuration;

        #endregion

        #region Class Methods

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "a";    // Replaces <email> with <a> tag
            var address = $"{to}@{Configuration["App:Domain"]}";
            output.Attributes.SetAttribute("href", $"mailto:{address}");
            output.Content.SetContent(address);
        }

        #endregion

    }

}

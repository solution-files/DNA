#region Usings

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace DNA3.Classes {

    public class SwaggerFilter : IDocumentFilter {

        #region Methods

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context) {
            List<System.Collections.Generic.KeyValuePair<string, OpenApiPathItem>> nonMobileRoutes = [.. swaggerDoc.Paths.Where(x => x.Key.ToString().ToLower().Contains("public"))];
            nonMobileRoutes.ForEach(x => { swaggerDoc.Paths.Remove(x.Key); });
        }

        #endregion

    }

}

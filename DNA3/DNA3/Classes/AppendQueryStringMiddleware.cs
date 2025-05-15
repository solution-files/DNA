#region Usings

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Diagnostics;
using System.Threading.Tasks;

#endregion

namespace DNA3.Classes {

    // To use this middleware, add UseAppendQueryString() as the last middleware of the pipeline in Program.cs
    #region Methods

    public class AppendQueryStringMiddleware(RequestDelegate next) {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context) {
            var url = context.Request.GetDisplayUrl();

            if (context.User.Identity.IsAuthenticated) {
                context.Session.SetString("returnUrl", url);
                Debug.Print(url);
            }

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }

    public static class AppendQueryStringMiddlewareExtensions {
        public static IApplicationBuilder UseAppendQueryString(
            this IApplicationBuilder builder) {
            return builder.UseMiddleware<AppendQueryStringMiddleware>();
        }
    }

    #endregion

}
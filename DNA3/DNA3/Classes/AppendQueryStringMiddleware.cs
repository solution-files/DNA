using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;

namespace DNA3.Classes {

    // To use this middleware, add UseAppendQueryString() as the last middleware of the pipeline in Program.cs

    public class AppendQueryStringMiddleware {
        private readonly RequestDelegate _next;

        public AppendQueryStringMiddleware(RequestDelegate next) {
            _next = next;
        }

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

}
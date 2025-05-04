#region Usings

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

#endregion

namespace Utilities {

    public class Http {

        #region Methods

        public static string GetBaseUrl(HttpContext context, string[] subfolders = default) {
            string result;
            try {
                result = $"{context.Request.Scheme}://{context.Request.Host}";
                foreach(string folder in subfolders) {
                    result = result + "/" + folder;
                }
            } catch(Exception ex) {
                result = ex.Message;
            }
            return result;
        }

        #endregion

    }

}

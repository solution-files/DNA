#region Usings

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Utilities {

    #region Interface

    public interface IHttp {

        string GetReferringUrl(HttpContext context, string name);
        bool SetReferringUrl(HttpContext context, string name);

    }

    #endregion

    #region Class Library

    public class Http : IHttp {

        #region Variables

        private readonly ILogger<Http> Logger;

        #endregion

        #region Methods

        public Http(ILogger<Http> logger) {
            Logger = logger;
        }

        public string GetReferringUrl(HttpContext context, string name) {
            string returnvalue = default;
            try {
                returnvalue = context.Session.GetString(name);
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return returnvalue;
        }

        public bool SetReferringUrl(HttpContext context, string name) {
            bool returnvalue = default;
            try {
                var referrer = context.Request.GetTypedHeaders().Referer.ToString();
                if (!string.IsNullOrEmpty(referrer)) {
                    context.Session.SetString(name, referrer);
                    returnvalue = true;
                } else {
                    returnvalue = false;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                returnvalue = false;
            }
            return returnvalue;
        }

        #endregion

    }

    #endregion

}

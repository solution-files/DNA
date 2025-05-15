#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

#endregion

namespace Utilities {
    public class Gemini {

        public static string EncodeFormData(Dictionary<string, string> data) {
            List<string> pairs = [];
            foreach (var pair in data) {
                pairs.Add($"{HttpUtility.UrlEncode(pair.Key)}={HttpUtility.UrlEncode(pair.Value)}");
            }
            return string.Join("&", pairs);
        }

    }

}

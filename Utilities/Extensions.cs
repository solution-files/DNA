#region Usings

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Globalization;

#endregion

namespace Utilities {

    #region Class

    public static class Extensions {

        #region Variables and Constants

        private static readonly string _paraBreak = "\r\n\r\n";
        private static readonly string _link = "<a href=\"{0}\">{1}</a>";
        private static readonly string _linkNoFollow = "<a href=\"{0}\" rel=\"nofollow\">{1}</a>";

        #endregion

        #region Mapping

        // Map To
        public static void MapTo(this object source, object destination) {
            if (source == null || destination == null) {
                throw new Exception("Source or/and destination objects are null");
            }
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps) {
                if (!srcProp.CanRead) {
                    continue;
                }
                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty == null) {
                    continue;
                }
                if (!targetProperty.CanWrite) {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate) {
                    continue;
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0) {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)) {
                    continue;
                }
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }

        #endregion

        #region Claims

        // Get Claim Value
        public static string GetClaimValue(this ClaimsIdentity claimsIdentity, string claimType) {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);
            return (claim != null) ? claim.Value : string.Empty;
        }

        // Has Sufficient Authority
        public static bool HasSufficientAuthority(this IPrincipal user, string RequestedAuthority) {
            bool Result = false;
            var RequestedLevel = RequestedAuthority switch {
                "Admin" => 3,
                "Manager" => 2,
                "User" => 1,
                _ => 0,
            };
            var ActualLevel = user.Role() switch {
                "Admin" => 3,
                "Manager" => 2,
                "User" => 1,
                _ => 0,
            };
            if (ActualLevel >= RequestedLevel) {
                Result = true;
            }

            return Result;
        }

        // Login Id
        public static int LoginId(this IPrincipal user) {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("lgnid");
            return claim == null ? 0 : Convert.ToInt32(claim.Value);
        }

        // User Id
        public static int UserId(this IPrincipal user) {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("usrid");
            return claim == null ? 0 : Convert.ToInt32(claim.Value);
        }

        // Client Id
        public static int ClientId(this IPrincipal user) {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("cliid");
            return claim == null ? 0 : Convert.ToInt32(claim.Value);
        }

        // First Name
        public static string FirstName(this IPrincipal user) {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("first");
            return claim?.Value;
        }

        // Last Name
        public static string LastName(this IPrincipal user) {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("last");
            return claim?.Value;
        }

        // Full Name
        public static string FullName(this IPrincipal user) {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("full");
            return claim?.Value;
        }

        // E-Mail Address
        public static string EmailAddress(this IPrincipal user) {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }

        // Role
        public static string Role(this IPrincipal user) {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("role");
            return claim?.Value;
        }

        // Thumbprint
        public static string Thumbprint(this IPrincipal user) {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("thumbprint");
            return claim?.Value;
        }

        #endregion

        #region HTML

        public static string ToHtml(this string s) {
            return ToHtml(s, false);
        }

        public static string ToHtml(this string s, bool nofollow) {
            StringBuilder sb = new();

            int pos = 0;
            while (pos < s.Length) {
                // Extract next paragraph
                int start = pos;
                pos = s.IndexOf(_paraBreak, start);
                if (pos < 0)
                    pos = s.Length;
                string para = s[start..pos].Trim();

                // Encode non-empty paragraph
                if (para.Length > 0)
                    EncodeParagraph(para, sb, nofollow);

                // Skip over paragraph break
                pos += _paraBreak.Length;
            }
            // Return result
            return sb.ToString();
        }

        private static void EncodeParagraph(string s, StringBuilder sb, bool nofollow) {
            // Start new paragraph
            sb.AppendLine("<p>");

            // HTML encode text
            s = HttpUtility.HtmlEncode(s);

            // Convert single newlines to <br>
            s = s.Replace(Environment.NewLine, "<br />\r\n");

            // Encode any hyperlinks
            EncodeLinks(s, sb, nofollow);

            // Close paragraph
            sb.AppendLine("\r\n</p>");
        }

        private static void EncodeLinks(string s, StringBuilder sb, bool nofollow) {
            // Parse and encode any hyperlinks
            int pos = 0;
            while (pos < s.Length) {
                // Look for next link
                int start = pos;
                pos = s.IndexOf("[[", pos);
                if (pos < 0)
                    pos = s.Length;
                // Copy text before link
                sb.Append(s.AsSpan(start, pos - start));
                if (pos < s.Length) {
                    string label, link;

                    start = pos + 2;
                    pos = s.IndexOf("]]", start);
                    if (pos < 0)
                        pos = s.Length;
                    label = s[start..pos];
                    int i = label.IndexOf("][");
                    if (i >= 0) {
                        link = label[(i + 2)..];
                        label = label[..i];
                    } else {
                        link = label;
                    }
                    // Append link
                    sb.Append(String.Format(nofollow ? _linkNoFollow : _link, link, label));

                    // Skip over closing "]]"
                    pos += 2;
                }
            }
        }

        #endregion

        #region Session

        // Set Object As JSON
        public static void SetObjectAsJson(this ISession session, string key, object value) {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        // Get Object From JSON
        public static T GetObjectFromJson<T>(this ISession session, string key) {
            var value = session.GetString(key);

            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        #endregion

        #region Strings

        // To Title Case
        public static string ToTitleCase(this string value) {
            string returnvalue = value;
            try {
                if (!string.IsNullOrEmpty(value)) {
                    returnvalue = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
                }
            } catch {
                // Do nothing for now
            }
            return returnvalue;
        }

        #endregion

    }

    #endregion

}

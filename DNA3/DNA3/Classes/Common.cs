#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace DNA3.Classes {

    #region Interface

    #endregion

    #region Class

    public class Common {

        #region Authentication

        // Get Claims List
        public static List<Claim> GetClaimsList(Login claimant) {
            List<Claim> claims = new();
            try {
                claims = new List<Claim> {
                    new Claim("lgnid", claimant.LoginId.ToString()),
                    new Claim("usrid", claimant.UserId.ToString()),
                    new Claim("cliid", claimant.User.ClientId.ToString()),
                    new Claim("first", claimant.User.First),
                    new Claim("last", claimant.User.Last),
                    new Claim("full", claimant.User.First + ' ' + claimant.User.Last),
                    new Claim("email", claimant.Email),
                    new Claim("role", claimant.User.Role.Code),
                    new Claim(ClaimTypes.Email, claimant.Email)
                };
            } catch (Exception ex) {
                Log.Error(ex, ex.Message);
            }
            return claims;
        }

        #endregion

        #region Cookies

        public static string GetCookie(HttpContext context, string name) {
            string value;
            try {
                value = context.Request.Cookies[name];
            } catch (Exception ex) {
                Common.Messages.Enqueue(ex.Message);
                Log.Error(ex, ex.Message);
                value = null;
            }
            return value;
        }

        public static bool SetCookie(HttpContext context, string name, string value) {
            bool result = true;
            try {
                if (GetCookie(context, name) != null) {
                    DropCookie(context, name);
                }
                context.Response.Cookies.Append(name, value);
            } catch (Exception ex) {
                Common.Messages.Enqueue(ex.Message);
                Log.Error(ex, ex.Message);
                result = false;
            }
            return result;
        }

        public static bool DropCookie(HttpContext context, string name) {
            bool result = true;
            try {
                context.Response.Cookies.Delete(name);
            } catch (Exception ex) {
                Common.Messages.Enqueue(ex.Message);
                Log.Error(ex, ex.Message);
                result = false;
            }
            return result;
        }

        #endregion

        #region Paswords and Hashing

        /// <summary>
        /// Generates a random password based on the options specified
        /// </summary>
        /// <param name="opts">Microsoft.AspNetCore.Identity.PasswordOptions</param>
        /// <returns>A random string</returns>
        public static string GenerateRandomPassword(PasswordOptions opts = null) {

            if (opts == null) {
                opts = new PasswordOptions() {
                    RequiredLength = 10,
                    RequiredUniqueChars = 1,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonAlphanumeric = true,
                    RequireUppercase = true
                };
            }

            string[] randomChars = new[] { "ABCDEFGHJKLMNOPQRSTUVWXYZ", "abcdefghijkmnopqrstuvwxyz", "0123456789", "!@$?_-" };

            Random rand = new(Environment.TickCount);
            List<char> chars = new();

            if (opts.RequireUppercase) {
                chars.Insert(rand.Next(0, chars.Count), randomChars[0][rand.Next(0, randomChars[0].Length)]);
            }

            if (opts.RequireLowercase) {
                chars.Insert(rand.Next(0, chars.Count), randomChars[1][rand.Next(0, randomChars[1].Length)]);
            }

            if (opts.RequireDigit) {
                chars.Insert(rand.Next(0, chars.Count), randomChars[2][rand.Next(0, randomChars[2].Length)]);
            }

            if (opts.RequireNonAlphanumeric) {
                chars.Insert(rand.Next(0, chars.Count), randomChars[3][rand.Next(0, randomChars[3].Length)]);
            }

            for (int i = chars.Count; i < opts.RequiredLength || chars.Distinct().Count() < opts.RequiredUniqueChars; i++) {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count), rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        /// <summary>
        /// Accepts a plain text string and returns a one-way hash suitable for storing in a database
        /// </summary>
        /// <param name="value">The plain-text string to be hashed</param>
        /// <returns>The one-way hashed value to be stored in a database</returns>
        public static String CreateHash(string value) {
            string result;
            try {
                result = (BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value))).Replace("-", "")).ToUpper();
            } catch {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Accepts a plain-text string and ensures that it matches the hashed version specified.
        /// </summary>
        /// <param name="password">The plain-text string to be evaluated</param>
        /// <param name="hash">The hashed value to be compared</param>
        /// <returns></returns>
        public static bool ValidatePassword(string password, string hash) {
            bool result = true;
            try {
                if (CreateHash(password) != hash.ToUpper()) {
                    result = false;
                }
            } catch {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// BackupFileName
        /// </summary>
        /// <param name="FileName">String representing the file name to format</param>
        /// <returns>Original File Name with Year, Month Date and Time prepended</returns>
        public static string BackupFileName(object FileName) {
            string ReturnValue;
            DateTime CurrentTime = DateTime.Now;
            try {
                ReturnValue = CurrentTime.Year.ToString() + "." + CurrentTime.Month.ToString("D2") + "." + CurrentTime.Day.ToString("D2") + "-" + CurrentTime.Hour.ToString("D2") + CurrentTime.Minute.ToString("D2") + "-" + FileName;
            } catch {
                ReturnValue = null;
            }
            return ReturnValue;
        }

        /// <summary>
        /// Nl2br
        /// </summary>
        /// <param name="text"></param>
        /// <returns>String with all new line characters replaced with <br /></returns>
        public static string Nl2br(string text) {
            return text.Replace("\r\n", "<br />").Replace("\n", "<br />");
        }

        /// <summary>
        /// BytesToString
        /// </summary>
        /// <param name="byteCount">The number to format as a Long Integer value</param>
        /// <returns>A human readable representation of the input value as a string</returns>
        /// <remarks>From StackOverflow.com</remarks>
        public static String BytesToString(long byteCount) {
            string[] suf = new[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0 " + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + " " + suf[place];
        }

        /// <summary>
        /// Megabytes
        /// </summary>
        /// <param name="Kilobytes">A double depicting the number of Kilobytes to evaluate</param>
        /// <returns>Double with input converted to Megabytes</returns>
        public static double Megabytes(double Kilobytes) {
            return Kilobytes / (double)1048576;
        }

        /// <summary>
        /// RightIndexOf - Retrieve the rightmost portion of a string beginning with the first or last occurance of the specified string index character
        /// </summary>
        /// <param name="SourceString">String containing the content to be evaluated</param>
        /// <param name="StringIndex">String depicting the index value to be located</param>
        /// <param name="LastOccurance">Boolean indicating if the first or last occurence of the index should be used for processing</param>
        /// <returns>The right n characters of the source string starting at the first or last occurence of a string index</returns>
        public static string RightIndexOf(string SourceString, string StringIndex, bool LastOccurance = true) {
            string ReturnValue = SourceString;
            try {
                int StartPosition;
                if (LastOccurance)
                    StartPosition = SourceString.LastIndexOf(StringIndex) + 1;
                else
                    StartPosition = SourceString.IndexOf(StringIndex) + 1;
                int EndPosition = SourceString.Length - StartPosition;
                ReturnValue = SourceString.Substring(StartPosition, EndPosition);
            } catch {
            }
            return ReturnValue;
        }

        /// <summary>
        /// LeftIndexOf - Retrieve the leftmost portion of a string ending with the first occurence of the specified string index character
        /// </summary>
        /// <param name="SourceString">String containging the content to be evaluated</param>
        /// <param name="StringIndex">String depicting the index value to be located</param>
        /// <returns>The left n characters of the source string ending with the first occurance of a string index</returns>
        public static string LeftIndexOf(string SourceString, string StringIndex) {
            string ReturnValue = SourceString;
            try {
                int EndPosition = SourceString.IndexOf(StringIndex);
                ReturnValue = SourceString.Substring(0, EndPosition);
            } catch {
            }
            return ReturnValue;
        }

        /// <summary>
        /// Numeric String Elements
        /// </summary>
        /// <param name="Elements"></param>
        /// <returns>The numeric portion of a string only</returns>
        public static string[] NumericStringElements(string[] Elements) {
            string[] ReturnValue = null;
            List<string> Result = new();
            try {
                foreach (var Element in Elements) {
                    for (var n = 1; n <= Element.Length; n++) {
                        if ("0123456789".IndexOf(Element.Substring(n, 1)) > 0) {
                            Result.Add(Element);
                            break;
                        }
                    }
                }
                if (Result.Count > 0)
                    ReturnValue = Result.ToArray();
            } catch {
                ReturnValue = null;
            }
            return ReturnValue;
        }

        /// <summary>
        /// NZ
        /// </summary>
        /// <param name="Value">Object to be tested</param>
        /// <param name="DefaultValue">Object respresenting the default value returned if null or DBNull</param>
        /// <returns> The specified default value if the input value is NULL, Zero or DBNULL (Similar to T-SQL ISNULL function)</returns>
        public static object Nz(object Value, object DefaultValue = null) {
            object ReturnValue = Value;
            try {
                if (Value == null || Value == DBNull.Value || Convert.ToString(Value).Length == 0) {
                    ReturnValue = DefaultValue;
                } else {
                    if ((Double)Value == 0) {
                        ReturnValue = DefaultValue;
                    }
                }
            } catch {
                // Do nothing
            }
            return ReturnValue;
        }

        /// <summary>
        /// Compares two object values and determines if they are within .001 of equality
        /// </summary>
        /// <param name="value1">Object defining the first value</param>
        /// <param name="value2">Object defining the second value</param>
        /// <returns></returns>
        public static bool EqualValue(object value1, object value2) {
            bool ReturnValue = false;
            try {
                if (Math.Abs((double)value1 - (double)value2) <= .001) {
                    ReturnValue = true;
                }
            } catch {
                // Ignore null values
            }
            return ReturnValue;
        }

        #endregion

        #region System Variables

        public static readonly Queue<string> Messages = new();
        public static readonly String CurrentFilter = "";
        public static readonly string Controller = "";
        public static readonly string Action = "";
        public static readonly string Mode = "";

        #endregion

    }

    #endregion

}

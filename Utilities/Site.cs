#region Usings

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace Utilities {

    public class Site {

        #region System Variables

        public static Queue<String> Messages = new Queue<String>();
        public static String CurrentFilter = "";
        public static string Controller = "";
        public static string Action = "";
        public static string Mode = "";
        public static Serilog.ILogger Log = null;
        public static string ConnectionString = "";

        #endregion

        #region Cookies

        public static string GetCookie(HttpContext context, string name) {
			string value;
			try {
                value = context.Request.Cookies[name];
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
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
                Site.Messages.Enqueue(ex.Message);
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
                Site.Messages.Enqueue(ex.Message);
                Log.Error(ex, ex.Message);
                result = false;
            }
            return result;
        }

        #endregion

        #region Paswords and Hashing

        /// <summary>
        /// Numeric String Elements
        /// </summary>
        /// <param name="Elements"></param>
        /// <returns>The numeric portion of a string only</returns>
        public static string[] NumericStringElements(string[] Elements) {
            string[] ReturnValue = null;
            List<string> Result = new List<string>();
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
        /// Clean String - Remove nulls and other non visible characters
        /// </summary>
        /// <param name="InputString">String representing the input to be cleansed</param>
        /// <returns>String cleanesed of all non-printable characters</returns>
        public static string CleanString(string InputString) {
            string ReturnValue ;
            try {
                ReturnValue = Encoding.ASCII.GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(Encoding.ASCII.EncodingName, new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback()), Encoding.UTF8.GetBytes(InputString)));
            } catch {
                ReturnValue = InputString;
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

    }

}

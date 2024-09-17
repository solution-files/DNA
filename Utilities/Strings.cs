#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Utilities {

    public static class Strings {

        #region General Purpose

        // Not Value Is Null
        public static string NotValueIsNull(object value) {
            string result;
            if (value is null) {
                result = "";
            } else {
                result = value.ToString();
            }
            return result;
        }

        // Split Camel Case
        public static string SplitCamelCase(string input) {
            string result;
            try {
                result = System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
            } catch (Exception ex) {
                result = ex.Message;
            }
            return result;
        }

        // Parse Tokens (Find tokens within a string eg [now])
        public static string[] ParseTokens(string content) {
            string result = "";
            string input;
            bool collect = false;
            try {
                foreach (char c in content) {
                    input = c.ToString();
                    if (input == "[") {
                        collect = true;
                    } else {
                        if (input == "]") {
                            result += input + ",";
                            collect = false;
                        }
                    }
                    if (collect) {
                        result += input;
                    }
                }
            } catch {
                result = content;
            }
            return result.TrimEnd(',').Split(",");
        }

        // Numeric value
        public static string NumericValue(string value) {
            string result;
            try {
                result = new String(value.Where(Char.IsDigit).ToArray());
            } catch {
                result = value;
            }
            return result;
        }

        // New Line to BR
        public static string Nl2br(string text) {
            return text.Replace("\r\n", "<br />").Replace("\n", "<br />");
        }

        // Bytes To String
        public static String BytesToString(long byteCount) {
            string[] suf = new[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0 " + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + " " + suf[place];
        }

        // Bytes From String
        public static byte[] BytesFromString(string str) {
            byte[] ReturnValue = default;
            try {
                ReturnValue = Encoding.ASCII.GetBytes(str);
            } catch {

            }
            return ReturnValue;
        }

        // Megabytes
        public static double Megabytes(double Kilobytes) {
            return Kilobytes / (double)1048576;
        }

        // Right Index Of
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

        // Left Index Of
        public static string LeftIndexOf(string SourceString, string StringIndex) {
            string ReturnValue = SourceString;
            try {
                int EndPosition = SourceString.IndexOf(StringIndex);
                ReturnValue = SourceString.Substring(0, EndPosition);
            } catch {
            }
            return ReturnValue;
        }

        // Backup File Name
        public static string BackupFileName(object FileName) {
            string ReturnValue;
            DateTime CurrentTime = DateTime.Now;
            try {
                ReturnValue = CurrentTime.Year.ToString() + "." + CurrentTime.Month.ToString("D2") + "." + CurrentTime.Day.ToString("D2") + "-" + FileName;
            } catch {
                ReturnValue = null;
            }
            return ReturnValue;
        }

        // Clean
        public static string Clean(string InputString) {
            string ReturnValue;
            try {
                ReturnValue = Encoding.ASCII.GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(Encoding.ASCII.EncodingName, new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback()), Encoding.UTF8.GetBytes(InputString))).Replace("\r\n", "").Replace("\t", "").Trim();
            } catch {
                ReturnValue = InputString;
            }
            return ReturnValue;
        }

        // Compacted
        public static string Compacted(string value, int maxLength) {
            string ellipsisChars = "...";
            char dirSeperatorChar = Path.DirectorySeparatorChar;
            string directorySeperator = dirSeperatorChar.ToString();
            if (value.Length <= maxLength)
                return value;
            int ellipsisLength = ellipsisChars.Length;
            if (maxLength <= ellipsisLength)
                return ellipsisChars;
            bool isFirstPartsTurn = true;
            string firstPart = "";
            string lastPart = "";
            int firstPartsUsed = 0;
            int lastPartsUsed = 0;
            string[] pathParts = value.Split(dirSeperatorChar);
            for (int i = 0; i <= pathParts.Length - 1; i++) {
                if (isFirstPartsTurn) {
                    string partToAdd = pathParts[firstPartsUsed] + directorySeperator;
                    if ((firstPart.Length + lastPart.Length + partToAdd.Length + ellipsisLength) > maxLength)
                        break;
                    firstPart += partToAdd;
                    if (partToAdd == directorySeperator) {
                    } else
                        isFirstPartsTurn = false;
                    firstPartsUsed += 1;
                } else {
                    int index = pathParts.Length - lastPartsUsed - 1;
                    string partToAdd = directorySeperator + pathParts[index];
                    if ((firstPart.Length + lastPart.Length + partToAdd.Length + ellipsisLength) > maxLength)
                        break;
                    lastPart = partToAdd + lastPart;
                    if (partToAdd == directorySeperator) {
                    } else
                        isFirstPartsTurn = true;
                    lastPartsUsed += 1;
                }
            }
            if (lastPart == "") {
                lastPart = pathParts[^1];
                lastPart = lastPart.Substring(lastPart.Length + ellipsisLength + firstPart.Length - maxLength, maxLength - ellipsisLength - firstPart.Length);
            }
            return Convert.ToString(firstPart + ellipsisChars) + lastPart;
        }

        // Base 64 Encode
        public static string Base64Encode(string value) {
            byte[] encodedBytes = System.Text.Encoding.Unicode.GetBytes(value);
            return Convert.ToBase64String(encodedBytes);
        }

        // Base 64 Decode
        public static string Base64Decode(string value) {
            byte[] decodedBytes = Convert.FromBase64String(Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(value)));
            return System.Text.Encoding.UTF8.GetString(decodedBytes);
        }

        #endregion

    }

}

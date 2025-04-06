#region "Usings"

using EBay.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Utilities;

#endregion

namespace EBay.Classes {

    public class Common {

        public static readonly Queue<string> Messages = new();
        public static readonly ILogger<Common> Logger;

        // Format Current Price
        public static string FormatCurrentPrice(EBay.Models.ItemSummary item) {
            string result = "";
            try {
                if (!string.IsNullOrEmpty(item.bidCount)) {
                    if (decimal.TryParse(item.currentBidPrice.value, out decimal value)) {
                        result = value.ToString("C2");
                    }
                } else {
                    if (decimal.TryParse(item.price.value, out decimal value)) {
                        result = value.ToString("C2");
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                Site.Messages.Enqueue(ex.Message);
            }
            return result;
        }

        // Format Condition
        public static string FormatCondition(EBay.Models.ItemSummary item) {
            string result = "";
            try {
                if (!string.IsNullOrEmpty(item.condition)) {
                    result = item.condition switch {
                        "New" => "Brand New",
                        _ => item.condition,
                    };
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Site.Messages.Enqueue(ex.Message);
            }
            return result;
        }

        // Format Shipping Details
        public static string FormatShippingDetails(EBay.Models.ItemSummary item) {
            string result = "";
            try {
                if (item.marketingPrice is not null) {
                    result += $"<span class=>Was: <del>{Convert.ToDecimal(item.marketingPrice.originalPrice.value):C2}</del> {item.marketingPrice.discountPercentage}% off</span>";
                }
                if (!string.IsNullOrEmpty(item.buyingOptions.ToString())) {
                    foreach (string option in item.buyingOptions) {
                        if (option.Contains("FIXED_PRICE")) {
                            result += "<br />Buy It Now";
                        }
                        if (option.Contains("BEST_OFFER")) {
                            result += "<br />or Best Offer";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(item.bidCount)) {
                    result += $"<br /><span>{item.bidCount} Bids</span>";
                } else {
                    result += "<br /><span>0 Bids</span>";
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Site.Messages.Enqueue(ex.Message);
            }
            return result;
        }

        // Format Listing Info
        public static string FormatListingInfo(EBay.Models.ItemSummary item) {
            string result = "";
            try {
                if (!string.IsNullOrEmpty(item.buyingOptions[0])) {
                    if (item.buyingOptions[0].Contains("Buy")) {
                        result = "Buy It Now";
                    } else {
                        result = "Auction";
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Site.Messages.Enqueue(ex.Message);
            }
            return result;
        }

        // Format Seller Info
        public static string FormatSellerInfo(EBay.Models.ItemSummary item) {
            string result = "";
            try {
                if (!string.IsNullOrEmpty(item.seller.username)) {
                    if(!string.IsNullOrEmpty(item.topRatedBuyingExperience)) {
                        result += $"<span class='text-danger fw-bold'><i class='fas fa-medal text-primary me-2'></i>Top Rated Buying Experience</span><br />";
                    }
                    result += $"{item.seller.username} ";
                    long feedbackscore = Convert.ToInt32(item.seller.feedbackScore);
                    result += feedbackscore switch {
                        < 10 => $"(<span class='text-danger font-weight-bold'>{feedbackscore}</span>) ",
                        < 100 => $"(<span class='text-warning font-weight-bold'>{feedbackscore}</span>) ",
                        _ => $"(<span class='text-success'>{feedbackscore}</span>) ",
                    };
                    decimal feedbackpercentage = Convert.ToDecimal(item.seller.feedbackPercentage);
                    result += feedbackpercentage switch {
                        < 90 => $"<span class='text-danger font-weight-bold'>{feedbackpercentage}</span>%",
                        < 96 => $"<span class='text-warning font-weight-bold'>{feedbackpercentage}</span>%",
                        _ => $"<span class='text-success'>{feedbackpercentage}</span>%",
                    };
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Site.Messages.Enqueue(ex.Message);
            }
            return result;
        }

        // Format Selling Status
        public static string FormatSellingStatus(EBay.Models.Item item) {
            string result = "";
            try {
                if (!string.IsNullOrEmpty(item.sellingStatus[0].timeLeft[0])) {
                    string value = item.sellingStatus[0].timeLeft[0];
                    string agg = "";
                    foreach (char c in value) {
                        int val;
                        switch (c.ToString()) {
                            case "D":
                                if (int.TryParse(agg, out val)) {
                                    if (val > 0) {
                                        if (val == 1) {
                                            result += $"{agg} Day ";
                                        } else {
                                            result += $"{agg} Days ";
                                        }
                                    }
                                }
                                agg = "";
                                break;
                            case "H":
                                if (int.TryParse(agg, out val)) {
                                    if (val > 0) {
                                        if (val == 1) {
                                            result += $"{agg} Hour ";
                                        } else {
                                            result += $"{agg} Hours ";
                                        }
                                    }
                                }
                                agg = "";
                                break;
                            case "M":
                                if (int.TryParse(agg, out val)) {
                                    if (val > 0) {
                                        if (val == 1) {
                                            result += $"{agg} Minute ";
                                        } else {
                                            result += $"{agg} Minutes ";
                                        }
                                    }
                                }
                                agg = "";
                                break;
                            case "S":
                                if (int.TryParse(agg, out val)) {
                                    if (val > 0) {
                                        if (val == 1) {
                                            result += $"{agg} Second ";
                                        } else {
                                            result += $"{agg} Seconds ";
                                        }
                                    }
                                }
                                agg = "";
                                break;
                            default:
                                if (int.TryParse(c.ToString(), out val)) {
                                    agg += c.ToString();
                                }
                                break;
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Site.Messages.Enqueue(ex.Message);
            }
            return result; ;
        }

        public static string SellingStatus(string value) {
            string result = value switch {
                "EndedWithSales" => "Sold",
                "EndedWithoutSales" => "No Sale",
                _ => value,
            };
            return result;
        }

        public static string BuildFilterXml(APIRequest apirequest) {
            string result = "";
            try {

                // Listing Type
                if (apirequest.ListingType != null) {
                    result += $"<itemFilter><name>ListingType</name><value>{apirequest.ListingType}</value></itemFilter>";
                }

                // Condition
                if (apirequest.Condition != null) {
                    result += $"<itemFilter><name>Condition</name><value>{apirequest.Condition}</value></itemFilter>";
                }

                // Sort Order
                if (apirequest.Sort != null) {
                    result += $"<sortOrder>{apirequest.Sort}</sortOrder>";
                }

                // Sold Items
                if (apirequest.Sold != null) {
                    result += $"<itemFilter><name>SoldItemsOnly</name><value>{apirequest.Sold}</value></itemFilter>";
                }

                // Completed Items
                if (apirequest.Completed != null) {
                    result += $"<itemFilter><name>CompletedItemsOnly</name><value>{apirequest.Completed}</value></itemFilter>";
                }

                // Returns Accepted
                if (apirequest.Returns != null) {
                    result += $"<itemFilter><name>ReturnsAcceptedOnly</name><value>{apirequest.Returns}</value></itemFilter>";
                }

                // Free Shipping
                if (apirequest.Shipping != null) {
                    result += $"<itemFilter><name>FreeShippingOnly</name><value>{apirequest.Shipping}</value></itemFilter>";
                }

                // Minimum Price
                if (apirequest.MinPrice != null) {
                    result += $"<itemFilter><name>MinPrice</name><value>{apirequest.MinPrice}</value></itemFilter>";
                }

                // Maximum Price
                if (apirequest.MaxPrice != null) {
                    result += $"<itemFilter><name>MaxPrice</name><value>{apirequest.MaxPrice}</value></itemFilter>";
                }

            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                ErrorMessage = ex.Message;
            }
            return result;
        }

        public static string ErrorMessage { get; set; }

        #region Common Methods

        /// <summary>
        /// Duration2String
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Duration2String(string Input = "P2DT0H19M6S") {
            string ReturnValue = "";
            try {
                string Type = "";
                String Value = "";
                string c = "";
                for (int x = 1; x < Input.Length; x++) {
                    c = Input.Substring(x, 1);
                    switch (c) {
                        case "P":
                            Type = "Period";
                            break;
                        case "T":
                            Type = "Time";
                            break;
                        case "Y":
                            ReturnValue += string.Format("{0}y ", Value);
                            Value = "";
                            break;
                        case "M":
                            if (Type == "P") {
                                ReturnValue += string.Format("{0}m ", Value);
                            } else {
                                ReturnValue += string.Format("{0}m ", Value);
                            }
                            Value = "";
                            break;
                        case "D":
                            ReturnValue += string.Format("{0}m ", Value);
                            Value = "";
                            break;
                        case "H":
                            ReturnValue += string.Format("{0}h ", Value);
                            Value = "";
                            break;
                        case "S":
                            ReturnValue += string.Format("{0}s", Value);
                            Value = "";
                            break;
                        default:
                            Value += c;
                            break;
                    }
                }
            } catch (Exception ex) {
                ErrorMessage = ex.Message;
                ReturnValue = "Invalid input format";
            }
            return ReturnValue;
        }

        /// <summary>
        /// GetResponse
        /// </summary>
        /// <param name="OperationName"></param>
        /// <param name="URL"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string GetResponse(string OperationName, string URL, string xml) {
            string result;
            try {
                byte[] Xml_bytes = System.Text.Encoding.UTF8.GetBytes(xml);
                WebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Headers.Add("X-EBAY-SOA-SERVICE-NAME", "FindingService");
                request.Headers.Add("X-EBAY-SOA-OPERATION-NAME", OperationName);
                request.Headers.Add("X-EBAY-SOA-SERVICE-VERSION", "1.13.0");
                request.Headers.Add("X-EBAY-SOA-GLOBAL-ID", "EBAY-US");
                request.Headers.Add("X-EBAY-SOA-SECURITY-APPNAME", "HostingX-e2bb-4a7b-ae2d-849dc38897f2");
                request.Headers.Add("X-EBAY-SOA-REQUEST-DATA-FORMAT", "XML");
                request.Headers.Add("X-EBAY-SOA-RESPONSE-DATA-FORMAT", "JSON");
                request.Method = "POST";
                request.ContentLength = Xml_bytes.Length;
                request.ContentType = "text/xml";
                System.IO.Stream requestStream = request.GetRequestStream();
                requestStream.Write(Xml_bytes, 0, Xml_bytes.Length);
                requestStream.Close();
                WebResponse response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd();
                return result;
            } catch (Exception ex) {
                ErrorMessage = ex.Message;
                result = ErrorMessage;
            }
            return result;
        }

        #endregion

    }

}

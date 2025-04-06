#region Usings

using EBay.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace EBay.Classes {

    #region Interface


    #endregion

    #region Class

    public class Formatting {

        #region Variables


        #endregion

        #region Showroom Methods

        public static string ShowroomPricingInfo(ItemSummary item) {
            string result = "";
            try {
                if (item.price is not null) {
                    if (item.buyingOptions[0].Contains("FIXED")) {
                        result += $@"<h5><i class=""fas fa-tag fa-flip-horizontal me-2""></i>{Convert.ToDecimal(item.price.value).ToString("N2")}</h5>";
                    } else {
                        if (item.buyingOptions[0].Contains("AUCTION")) {
                            result += $@"<h5><i class=""fas fa-tag fa-flip-horizontal me-2""></i>{Convert.ToDecimal(item.price.value).ToString("N2")}</h5>";
                        } else {
                            result += $@"<h5><i class=""fas fa-tag fa-flip-horizontal me-2""></i>{Convert.ToDecimal(item.price.value).ToString("N2")}</h5>";
                        }
                    }
                } else {
                    result += "<h5>0.00</h5";
                }
            } catch (Exception ex) {
                Log.Logger.Error(ex, ex.Message);
            }
            return result;
        }

        public static string ShowroomAuctionInfo(ItemSummary item) {
            string result = "";
            try {
                if (item.currentBidPrice is null) {
                    result += $@"<h5><i class=""fas fa-gavel mr-2""></i>$0.00</h5>";
                } else {
                    result += $@"<h5><i class=""fas fa-gavel mr-2""></i>{item.currentBidPrice.currency}{Convert.ToDecimal(item.currentBidPrice.value).ToString("N2")}</h5>";
                }
            } catch (Exception ex) {
                Log.Logger.Error(ex, ex.Message);
            }
            return result;
        }

        public static string ShowroomListingInfo(ItemSummary item) {
            string result = "";
            try {
                result += $@"<span class=""small""><i class=""far fa-clock""></i></span><br />";

                if (item.itemEndDate is null) {
                    result += $@"<span class=""small"">Until Sold</span>";
                } else {
                    TimeSpan interval = DateTime.UtcNow - Convert.ToDateTime(item.itemEndDate);
                    result += $@"
                        <span class=""small"">{string.Format("{0}D {1}H {2}M {3}S", interval.Days, interval.Hours, interval.Minutes, interval.Seconds)}</span>;
                    ";
                }
            } catch (Exception ex) {
                Log.Logger.Error(ex, ex.Message);
            }
            return result;
        }

        public static string ShowroomSellerInfo(ItemSummary item) {
            string result = "";
            try {
                result += $@"
                    <span class=""small"">{item.seller.username}</span><br />
                    <span class=""small"">({item.seller.feedbackScore})</span>
                ";
            } catch (Exception ex) {
                Log.Logger.Error(ex, ex.Message);
            }
            return result;
        }

        #endregion

    }

    #endregion

}

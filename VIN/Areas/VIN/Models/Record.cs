#region Usings

using VIN.Models;

#endregion

#nullable disable

namespace VIN.Models {

    public class Record {

        #region Model Attributes

        public int id { get; set; }
        public string vin { get; set; }
        public string displayColor { get; set; }
        public int year { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string price { get; set; }
        public string mileage { get; set; }
        public string city { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string primaryPhotoUrl { get; set; }
        public string condition { get; set; }
        public int providerId { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int modelId { get; set; }
        public string dealerName { get; set; }
        public bool active { get; set; }
        public string state { get; set; }
        public string trim { get; set; }
        public string clickoffUrl { get; set; }
        public bool acceptsLeads { get; set; }
        public string bodyType { get; set; }
        public string bodyStyle { get; set; }
        public object regionName { get; set; }
        public string experience { get; set; }
        public bool requiresAddressWithLead { get; set; }
        public TrackingParams trackingParams { get; set; }
        public int providerGroupId { get; set; }
        public int mileageUnformatted { get; set; }
        public string mileageHumanized { get; set; }
        public string priceMobile { get; set; }
        public int priceUnformatted { get; set; }
        public bool recentPriceDrop { get; set; }
        public string vdpUrl { get; set; }
        public bool showNewMileage { get; set; }
        public bool eligibleForFinancing { get; set; }
        public string financingExperience { get; set; }
        public List<string> photoUrls { get; set; }
        public bool isHot { get; set; }
        public string hrefTarget { get; set; }
        public int distanceFromOrigin { get; set; }
        public object humanizedSearchLocation { get; set; }
        public object hideDistance { get; set; }
        public object noPriceText { get; set; }
        public object target { get; set; }
        public int monthlyPayment { get; set; }
        public bool clickOff { get; set; }
        public bool emailOptDefault { get; set; }
        public bool showThankyouPage { get; set; }
        public bool showRsrp { get; set; }
        public bool allowOneClickSubmit { get; set; }
        public bool paidAllowOneClickSubmit { get; set; }
        public bool newPriceAsMsrp { get; set; }
        public bool preCheckThankyou { get; set; }
        public bool preCheckThankyouMobile { get; set; }
        public bool openInNewWindow { get; set; }
        public bool alwaysAskForZip { get; set; }
        public bool requireEmailOptIn { get; set; }
        public string providerName { get; set; }
        public bool availableNationwide { get; set; }
        public bool regional { get; set; }
        public string thumbnailUrl { get; set; }
        public string thumbnailUrlLarge { get; set; }
        public bool quickPicksEligible { get; set; }
        public string dealerGroupUuid { get; set; }
        public int cplValue { get; set; }
        public string partnerType { get; set; }

        #endregion

    }

}

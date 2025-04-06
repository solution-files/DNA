// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Collections.Generic;

namespace EBay.Models.Insights {

    public class AdditionalImage {
        public string height { get; set; }
        public string imageUrl { get; set; }
        public string width { get; set; }
    }

    public class AspectDistribution {
        public List<AspectValueDistribution> aspectValueDistributions { get; set; }
        public string localizedAspectName { get; set; }
    }

    public class AspectValueDistribution {
        public string localizedAspectValue { get; set; }
        public string matchCount { get; set; }
        public string refinementHref { get; set; }
    }

    public class BuyingOptionDistribution {
        public string buyingOption { get; set; }
        public string matchCount { get; set; }
        public string refinementHref { get; set; }
    }

    public class Category {
        public string categoryId { get; set; }
    }

    public class CategoryDistribution {
        public string categoryId { get; set; }
        public string categoryName { get; set; }
        public string matchCount { get; set; }
        public string refinementHref { get; set; }
    }

    public class ConditionDistribution {
        public string condition { get; set; }
        public string conditionId { get; set; }
        public string matchCount { get; set; }
        public string refinementHref { get; set; }
    }

    public class Image {
        public string height { get; set; }
        public string imageUrl { get; set; }
        public string width { get; set; }
    }

    public class ItemLocation {
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string county { get; set; }
        public string postalCode { get; set; }
        public string stateOrProvince { get; set; }
    }

    public class ItemSale {
        public List<AdditionalImage> additionalImages { get; set; }
        public string adultOnly { get; set; }
        public string bidCount { get; set; }
        public List<string> buyingOptions { get; set; }
        public List<Category> categories { get; set; }
        public string condition { get; set; }
        public string conditionId { get; set; }
        public string epid { get; set; }
        public Image image { get; set; }
        public string itemAffiliateWebUrl { get; set; }
        public string itemGroupHref { get; set; }
        public string itemGroupType { get; set; }
        public string itemHref { get; set; }
        public string itemId { get; set; }
        public ItemLocation itemLocation { get; set; }
        public string itemWebUrl { get; set; }
        public string lastSoldDate { get; set; }
        public LastSoldPrice lastSoldPrice { get; set; }
        public Seller seller { get; set; }
        public List<ThumbnailImage> thumbnailImages { get; set; }
        public string title { get; set; }
        public string totalSoldQuantity { get; set; }
    }

    public class LastSoldPrice {
        public string convertedFromCurrency { get; set; }
        public string convertedFromValue { get; set; }
        public string currency { get; set; }
        public string value { get; set; }
    }

    public class Refinement {
        public List<AspectDistribution> aspectDistributions { get; set; }
        public List<BuyingOptionDistribution> buyingOptionDistributions { get; set; }
        public List<CategoryDistribution> categoryDistributions { get; set; }
        public List<ConditionDistribution> conditionDistributions { get; set; }
        public string dominantCategoryId { get; set; }
    }

    public class Root {
        public string href { get; set; }
        public List<ItemSale> itemSales { get; set; }
        public string limit { get; set; }
        public string next { get; set; }
        public string offset { get; set; }
        public string prev { get; set; }
        public Refinement refinement { get; set; }
        public string total { get; set; }
    }

    public class Seller {
        public string feedbackPercentage { get; set; }
        public string feedbackScore { get; set; }
        public string username { get; set; }
    }

    public class ThumbnailImage {
        public string height { get; set; }
        public string imageUrl { get; set; }
        public string width { get; set; }
    }

}


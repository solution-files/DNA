// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Collections.Generic;

namespace EBay.Models {

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

    public class AutoCorrections {
        public string q { get; set; }
    }

    public class BuyingOptionDistribution {
        public string buyingOption { get; set; }
        public string matchCount { get; set; }
        public string refinementHref { get; set; }
    }

    public class Category {
        public string categoryId { get; set; }
        public string categoryName { get; set; }
    }

    public class CategoryDistribution {
        public string categoryId { get; set; }
        public string categoryName { get; set; }
        public string matchCount { get; set; }
        public string refinementHref { get; set; }
    }

    public class CompatibilityProperty {
        public string localizedName { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class ConditionDistribution {
        public string condition { get; set; }
        public string conditionId { get; set; }
        public string matchCount { get; set; }
        public string refinementHref { get; set; }
    }

    public class CurrentBidPrice {
        public string convertedFromCurrency { get; set; }
        public string convertedFromValue { get; set; }
        public string currency { get; set; }
        public string value { get; set; }
    }

    public class DiscountAmount {
        public string convertedFromCurrency { get; set; }
        public string convertedFromValue { get; set; }
        public string currency { get; set; }
        public string value { get; set; }
    }

    public class DistanceFromPickupLocation {
        public string unitOfMeasure { get; set; }
        public string value { get; set; }
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

    public class ItemSummary {
        public List<AdditionalImage> additionalImages { get; set; }
        public string adultOnly { get; set; }
        public string availableCoupons { get; set; }
        public string bidCount { get; set; }
        public List<string> buyingOptions { get; set; }
        public List<Category> categories { get; set; }
        public string compatibilityMatch { get; set; }
        public List<CompatibilityProperty> compatibilityProperties { get; set; }
        public string condition { get; set; }
        public string conditionId { get; set; }
        public CurrentBidPrice currentBidPrice { get; set; }
        public DistanceFromPickupLocation distanceFromPickupLocation { get; set; }
        public string energyEfficiencyClass { get; set; }
        public string epid { get; set; }
        public Image image { get; set; }
        public string itemAffiliateWebUrl { get; set; }
        public string itemCreationDate { get; set; }
        public string itemEndDate { get; set; }
        public string itemGroupHref { get; set; }
        public string itemGroupType { get; set; }
        public string itemHref { get; set; }
        public string itemId { get; set; }
        public ItemLocation itemLocation { get; set; }
        public string itemWebUrl { get; set; }
        public List<string> leafCategoryIds { get; set; }
        public string legacyItemId { get; set; }
        public string listingMarketplaceId { get; set; }
        public MarketingPrice marketingPrice { get; set; }
        public List<PickupOption> pickupOptions { get; set; }
        public Price price { get; set; }
        public string priceDisplayCondition { get; set; }
        public string priorityListing { get; set; }
        public List<string> qualifiedPrograms { get; set; }
        public Seller seller { get; set; }
        public List<ShippingOption> shippingOptions { get; set; }
        public string shortDescription { get; set; }
        public List<ThumbnailImage> thumbnailImages { get; set; }
        public string title { get; set; }
        public string topRatedBuyingExperience { get; set; }
        public string tyreLabelImageUrl { get; set; }
        public UnitPrice unitPrice { get; set; }
        public string unitPricingMeasure { get; set; }
        public string watchCount { get; set; }
    }

    public class MarketingPrice {
        public DiscountAmount discountAmount { get; set; }
        public string discountPercentage { get; set; }
        public OriginalPrice originalPrice { get; set; }
        public string priceTreatment { get; set; }
    }

    public class OriginalPrice {
        public string convertedFromCurrency { get; set; }
        public string convertedFromValue { get; set; }
        public string currency { get; set; }
        public string value { get; set; }
    }

    public class Parameter {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class PickupOption {
        public string pickupLocationType { get; set; }
    }

    public class Price {
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
        public AutoCorrections autoCorrections { get; set; }
        public string href { get; set; }
        public List<ItemSummary> itemSummaries { get; set; }
        public string limit { get; set; }
        public string next { get; set; }
        public string offset { get; set; }
        public string prev { get; set; }
        public Refinement refinement { get; set; }
        public string total { get; set; }
        public List<Warning> warnings { get; set; }
    }

    public class Seller {
        public string feedbackPercentage { get; set; }
        public string feedbackScore { get; set; }
        public string sellerAccountType { get; set; }
        public string username { get; set; }
    }

    public class ShippingCost {
        public string convertedFromCurrency { get; set; }
        public string convertedFromValue { get; set; }
        public string currency { get; set; }
        public string value { get; set; }
    }

    public class ShippingOption {
        public string guaranteedDelivery { get; set; }
        public string maxEstimatedDeliveryDate { get; set; }
        public string minEstimatedDeliveryDate { get; set; }
        public ShippingCost shippingCost { get; set; }
        public string shippingCostType { get; set; }
    }

    public class ThumbnailImage {
        public string height { get; set; }
        public string imageUrl { get; set; }
        public string width { get; set; }
    }

    public class UnitPrice {
        public string convertedFromCurrency { get; set; }
        public string convertedFromValue { get; set; }
        public string currency { get; set; }
        public string value { get; set; }
    }

    public class Warning {
        public string category { get; set; }
        public string domain { get; set; }
        public string errorId { get; set; }
        public List<string> inputRefIds { get; set; }
        public string longMessage { get; set; }
        public string message { get; set; }
        public List<string> outputRefIds { get; set; }
        public List<Parameter> parameters { get; set; }
        public string subdomain { get; set; }
    }

}
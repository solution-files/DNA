// Inventory myDeserializedClass = JsonConvert.DeserializeObject<Inventory>(myJsonResponse);
using System.Collections.Generic;

namespace EBay.Models {

    public class AllocationByFormat {
        public string auction { get; set; }
        public string fixedPrice { get; set; }
    }

    public class Availability {
        public List<PickupAtLocationAvailability> pickupAtLocationAvailability { get; set; }
        public ShipToLocationAvailability shipToLocationAvailability { get; set; }
    }

    public class AvailabilityDistribution {
        public FulfillmentTime fulfillmentTime { get; set; }
        public string merchantLocationKey { get; set; }
        public string quantity { get; set; }
    }

    public class ConditionDescriptor {
        public string additionalInfo { get; set; }
        public string name { get; set; }
        public List<string> values { get; set; }
    }

    public class Dimensions {
        public string height { get; set; }
        public string length { get; set; }
        public string unit { get; set; }
        public string width { get; set; }
    }

    public class FulfillmentTime {
        public string unit { get; set; }
        public string value { get; set; }
    }

    public class InventoryItem {
        public Availability availability { get; set; }
        public string condition { get; set; }
        public string conditionDescription { get; set; }
        public List<ConditionDescriptor> conditionDescriptors { get; set; }
        public List<string> groupIds { get; set; }
        public List<string> inventoryItemGroupKeys { get; set; }
        public string locale { get; set; }
        public PackageWeightAndSize packageWeightAndSize { get; set; }
        public Product product { get; set; }
        public string sku { get; set; }
    }

    public class PackageWeightAndSize {
        public Dimensions dimensions { get; set; }
        public string packageType { get; set; }
        public string shippingIrregular { get; set; }
        public Weight weight { get; set; }
    }

    public class PickupAtLocationAvailability {
        public string availabilityType { get; set; }
        public FulfillmentTime fulfillmentTime { get; set; }
        public string merchantLocationKey { get; set; }
        public string quantity { get; set; }
    }

    public class Product {
        public string aspects { get; set; }
        public string brand { get; set; }
        public string description { get; set; }
        public List<string> ean { get; set; }
        public string epid { get; set; }
        public List<string> imageUrls { get; set; }
        public List<string> isbn { get; set; }
        public string mpn { get; set; }
        public string subtitle { get; set; }
        public string title { get; set; }
        public List<string> upc { get; set; }
        public List<string> videoIds { get; set; }
    }
    public class Inventory {
        public string href { get; set; }
        public List<InventoryItem> inventoryItems { get; set; }
        public string limit { get; set; }
        public string next { get; set; }
        public string prev { get; set; }
        public string size { get; set; }
        public string total { get; set; }
    }

    public class ShipToLocationAvailability {
        public AllocationByFormat allocationByFormat { get; set; }
        public List<AvailabilityDistribution> availabilityDistributions { get; set; }
        public string quantity { get; set; }
    }

    public class Weight {
        public string unit { get; set; }
        public string value { get; set; }
    }

}
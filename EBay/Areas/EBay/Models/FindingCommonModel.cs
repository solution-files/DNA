#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace EBay.Models {

	#region Common Response Structure

	public class PrimaryCategory {
		public IList<string> categoryId { get; set; }
		public IList<string> categoryName { get; set; }
	}

	public class GalleryURL {
		public string @gallerySize { get; set; }
		public string __value__ { get; set; }
	}

	public class GalleryInfoContainer {
		public IList<GalleryURL> galleryURL { get; set; }
	}

	public class SellerInfo {
		public IList<string> sellerUserName { get; set; }
		public IList<string> feedbackScore { get; set; }
		public IList<string> positiveFeedbackPercent { get; set; }
		public IList<string> feedbackRatingStar { get; set; }
		public IList<string> topRatedSeller { get; set; }
	}

	public class ShippingServiceCost {
		public string @currencyId { get; set; }
		public string __value__ { get; set; }
	}

	public class ShippingInfo {
		[DisplayFormat(DataFormatString = "{0:C}")]
		public IList<ShippingServiceCost> shippingServiceCost { get; set; }
		public IList<string> shippingType { get; set; }
		public IList<string> shipToLocations { get; set; }
		public IList<string> expeditedShipping { get; set; }
		public IList<string> oneDayShippingAvailable { get; set; }
		public IList<string> handlingTime { get; set; }
	}

	public class CurrentPrice {
		public string @currencyId { get; set; }
		public string __value__ { get; set; }
	}

	public class ConvertedCurrentPrice {
		public string @currencyId { get; set; }
		public string __value__ { get; set; }
	}

	public class SellingStatu {
		[DisplayFormat(DataFormatString = "{0:C}")]
		public IList<CurrentPrice> currentPrice { get; set; }
		public IList<ConvertedCurrentPrice> convertedCurrentPrice { get; set; }
		public IList<string> sellingState { get; set; }
		public IList<string> timeLeft { get; set; }
	}

	public class ListingInfo {
		public IList<string> bestOfferEnabled { get; set; }
		public IList<string> buyItNowAvailable { get; set; }
		public IList<DateTime> startTime { get; set; }
		public IList<DateTime> endTime { get; set; }
		public IList<string> listingType { get; set; }
		public IList<string> gift { get; set; }
		public IList<string> watchCount { get; set; }
	}

	public class Condition {
		public IList<string> conditionId { get; set; }
		public IList<string> conditionDisplayName { get; set; }
	}

	public class StoreInfo {
		public IList<string> storeName { get; set; }
		public IList<string> storeURL { get; set; }
	}

	public class Item {
		public IList<string> itemId { get; set; }
		public IList<string> title { get; set; }
		public IList<string> globalId { get; set; }
		public IList<PrimaryCategory> primaryCategory { get; set; }
		public IList<string> galleryURL { get; set; }
		public IList<GalleryInfoContainer> galleryInfoContainer { get; set; }
		public IList<string> viewItemURL { get; set; }
		public IList<string> paymentMethod { get; set; }
		public IList<string> autoPay { get; set; }
		public IList<string> postalCode { get; set; }
		public IList<string> location { get; set; }
		public IList<string> country { get; set; }
		public IList<SellerInfo> sellerInfo { get; set; }
		public IList<ShippingInfo> shippingInfo { get; set; }
		public IList<SellingStatu> sellingStatus { get; set; }
		public IList<ListingInfo> listingInfo { get; set; }
		public IList<string> returnsAccepted { get; set; }
		public IList<Condition> condition { get; set; }
		public IList<string> isMultiVariationListing { get; set; }
		public IList<string> pictureURLSuperSize { get; set; }
		public IList<string> pictureURLLarge { get; set; }
		public IList<string> topRatedListing { get; set; }
		public IList<StoreInfo> storeInfo { get; set; }
	}

	public class SearchResult {
		public string @count { get; set; }
		public IList<Item> item { get; set; }
	}

	public class PaginationOutput {
		public IList<string> pageNumber { get; set; }
		public IList<string> entriesPerPage { get; set; }
		public IList<string> totalPages { get; set; }
		public IList<string> totalEntries { get; set; }
	}

	public class ChildCategoryHistogram {
		public IList<string> categoryId { get; set; }
		public IList<string> categoryName { get; set; }
		public IList<string> count { get; set; }
	}

	public class CategoryHistogram {
		public IList<string> categoryId { get; set; }
		public IList<string> categoryName { get; set; }
		public IList<string> count { get; set; }
		public IList<ChildCategoryHistogram> childCategoryHistogram { get; set; }
	}

	public class CategoryHistogramContainer {
		public IList<CategoryHistogram> categoryHistogram { get; set; }
	}

	#endregion

	#region Error Response Structure

	public class Error {
		public IList<string> errorId { get; set; }
		public IList<string> domain { get; set; }
		public IList<string> severity { get; set; }
		public IList<string> category { get; set; }
		public IList<string> message { get; set; }
		public IList<string> subdomain { get; set; }
	}

	public class ErrorMessage {
		public IList<Error> error { get; set; }
	}

	#endregion

}

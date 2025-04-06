#region Usings

using EBay.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace EBay.Classes {

	public class FindCompletedItems {

		#region Properties, Variables and Constants

		public string Keywords { get; set; }
		public string Condition { get; set; }
		public string SortOrder { get; set; }
		public string ListingType { get; set; }
		public string SoldItems { get; set; }
		public string CompletedItems { get; set; }
		public string FreeShipping { get; set; }
		public string ReturnsAccepted { get; set; }

		public string Ack { get; set; }
		public int PageNumber { get; set; }
		public int EntriesPerPage { get; set; }
		public int TotalEntries { get; set; }
		public int TotalPages { get; set; }
		public IEnumerable<Item> Items { get; set; }
		string ErrorMessage { get; set; }

		#endregion

		#region Class Methods

		// Constructor
		public FindCompletedItems() {

			Keywords = "";
			Condition = "";
			SortOrder = "";
			SoldItems = "";
			CompletedItems = "";
			FreeShipping = "";
			ReturnsAccepted = "";

			PageNumber = 1;
			EntriesPerPage = 15;
			TotalEntries = 0;
			TotalPages = 0;

			Ack = "Pending";
			Items = Enumerable.Empty<Item>();
			ErrorMessage = "";

		}

		// Request
		public void Request() {
			try {
				string operation = "findCompletedItems";
				string url = "https://svcs.ebay.com/services/search/FindingService/v1";
				string xml = $@"<?xml version='1.0' encoding='utf-8'?>
                <findCompletedItemsRequest xmlns='http://www.ebay.com/marketplace/search/v1/services'>
                <keywords>{Keywords}</keywords>
                <outputSelector>AspectHistogram</outputSelector>
                <outputSelector>CategoryHistogram</outputSelector>
                <outputSelector>ConditionHistogram</outputSelector>
                <outputSelector>GalleryInfo</outputSelector>
                <outputSelector>PictureURLLarge</outputSelector>
                <outputSelector>PictureURLSuperSize</outputSelector>
                <outputSelector>SellerInfo</outputSelector>
                <outputSelector>StoreInfo</outputSelector>
                <outputSelector>UnitPriceInfo</outputSelector>
                <sortOrder>{SortOrder}</sortOrder>
                <itemFilter>
                    <name>Condition</name>
                    <value>{Condition}</value>
                </itemFilter>
                <itemFilter>
                    <name>FreeShippingOnly</name>
                    <value>{FreeShipping}</value>
                </itemFilter>
                <itemFilter>
                    <name>SoldItemsOnly</name>
                    <value>{SoldItems}</value>
                </itemFilter>
                <itemFilter>
                    <name>CompletedItemsOnly</name>
                    <value>{CompletedItems}</value>
                </itemFilter>
                <sortOrder>{SortOrder}</sortOrder>
                <paginationInput>
                    <entriesPerPage>{EntriesPerPage}</entriesPerPage>
                    <pageNumber>{PageNumber}</pageNumber>
                </paginationInput>
                </findCompletedItemsRequest>";

				Debug.WriteLine(xml);

				string doc = EBay.Classes.Common.GetResponse(operation, url, xml);
				Debug.WriteLine(doc);

				FindCompletedItemsModel RootObject = JsonConvert.DeserializeObject<FindCompletedItemsModel>(doc);
				List<FindCompletedItemsResponse> Response = RootObject.findCompletedItemsResponse.ToList();

				//foreach (FindCompletedItemsResponse r in Response) {
				//    List<SearchResult> Result = r.searchResult.ToList();
				//    foreach (SearchResult s in Result) {
				//        Items = s.item.ToList();
				//    }
				//}

				Ack = Response[0].ack[0];
				PageNumber = Convert.ToInt32(Response[0].paginationOutput[0].pageNumber[0]);
				EntriesPerPage = Convert.ToInt32(Response[0].paginationOutput[0].entriesPerPage[0]);
				TotalEntries = Convert.ToInt32(Response[0].paginationOutput[0].totalEntries[0]);
				TotalPages = Convert.ToInt32(Response[0].paginationOutput[0].totalPages[0]);
				Items = Response[0].searchResult[0].item.ToList();

			} catch (Exception ex) {
				ErrorMessage = ex.Message;
			}
		}

		// Initialize
		private void Initialize() {
			Ack = "Pending";
			PageNumber = 1;
			EntriesPerPage = 15;
			TotalEntries = 0;
			TotalPages = 0;
			Items = Enumerable.Empty<Item>();
			ErrorMessage = "";
		}

		#endregion

	}

}

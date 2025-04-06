#region "Usings"

using EBay.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace EBay.Classes {

	public class FindItemsByKeywords {

		#region "Properties, Variables and Constants"

		//public string Keywords { get; set; }
		//public string Condition { get; set; }
		//public string SortOrder { get; set; }
		//public string SoldItems { get; set; }
		//public string CompletedItems { get; set; }
		//public string FreeShipping { get; set; }
		//public string ReturnsAccepted { get; set; }
		//public string Ack { get; set; }
		//public int PageNumber { get; set; }
		//public int EntriesPerPage { get; set; }
		//public int TotalEntries { get; set; }
		//public int TotalPages { get; set; }
		//public IEnumerable<Item> Items { get; set; }
		//string ErrorMessage { get; set; }

		public APIRequest apirequest { get; set; }

		#endregion

		#region "Class Methods"

		// Constructor
		public FindItemsByKeywords(ref APIRequest apiRequest) {

			//Keywords = "";
			//Condition = "";
			//SortOrder = "";
			//SoldItems = "";
			//CompletedItems = "";
			//FreeShipping = "";
			//ReturnsAccepted = "";
			//Ack = "Pending";
			//PageNumber = 1;
			//EntriesPerPage = 10;
			//TotalEntries = 0;
			//TotalPages = 0;
			//Items = Enumerable.Empty<Item>();
			//ErrorMessage = "";

			apirequest = apiRequest;

		}

		// Request
		public void Request() {
			try {
				string operation = "findItemsByKeywords";
				string url = "https://svcs.ebay.com/services/search/FindingService/v1";
				string xml = $@"<?xml version='1.0' encoding='utf-8'?>
                <findItemsByKeywordsRequest xmlns='http://www.ebay.com/marketplace/search/v1/services'>
                    <keywords>{apirequest.Keywords}</keywords>
                    <outputSelector>AspectHistogram</outputSelector>
                    <outputSelector>CategoryHistogram</outputSelector>
                    <outputSelector>ConditionHistogram</outputSelector>
                    <outputSelector>GalleryInfo</outputSelector>
                    <outputSelector>PictureURLLarge</outputSelector>
                    <outputSelector>PictureURLSuperSize</outputSelector>
                    <outputSelector>SellerInfo</outputSelector>
                    <outputSelector>StoreInfo</outputSelector>
                    <outputSelector>UnitPriceInfo</outputSelector>
                    {Common.BuildFilterXml(apirequest)}
                    <paginationInput>
                        <entriesPerPage>{apirequest.EntriesPerPage}</entriesPerPage>
                        <pageNumber>{apirequest.PageNumber}</pageNumber>
                    </paginationInput>
                </findItemsByKeywordsRequest>";

				xml = xml.Replace("\n", "").Replace("\r", "");

				Debug.WriteLine(xml);

				string doc = EBay.Classes.Common.GetResponse(operation, url, xml);
				Debug.WriteLine(doc);

				FindItemsByKeywordsModel RootObject = JsonConvert.DeserializeObject<FindItemsByKeywordsModel>(doc);
				List<FindItemsByKeywordsResponse> Response = RootObject.findItemsByKeywordsResponse.ToList();

				apirequest.Ack = Response[0].ack[0];
				apirequest.PageNumber = Convert.ToInt32(Response[0].paginationOutput[0].pageNumber[0]);
				apirequest.EntriesPerPage = Convert.ToInt32(Response[0].paginationOutput[0].entriesPerPage[0]);
				apirequest.TotalEntries = Convert.ToInt32(Response[0].paginationOutput[0].totalEntries[0]);
				apirequest.TotalPages = Convert.ToInt32(Response[0].paginationOutput[0].totalPages[0]);
				apirequest.Items = Response[0].searchResult[0].item.ToList();

			} catch (Exception ex) {
				apirequest.ErrorMessage = ex.Message;
			}
		}

		#endregion

	}

}

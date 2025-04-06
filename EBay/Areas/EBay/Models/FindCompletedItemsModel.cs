#region "Usings"

using System;
using System.Collections.Generic;

#endregion

namespace EBay.Models {

	#region "Response Structure"

	public class FindCompletedItemsModel {
		public IList<FindCompletedItemsResponse> findCompletedItemsResponse { get; set; }
	}

	public class FindCompletedItemsResponse {
		public IList<string> ack { get; set; }
		public IList<string> version { get; set; }
		public IList<DateTime> timestamp { get; set; }
		public IList<SearchResult> searchResult { get; set; }
		public IList<PaginationOutput> paginationOutput { get; set; }
	}

	#endregion

}

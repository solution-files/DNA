#region "Usings"

using System;
using System.Collections.Generic;

#endregion

namespace EBay.Models {

	#region "Response Structure"

	public class FindItemsByKeywordsResponse {
		public IList<string> ack { get; set; }
		public IList<string> version { get; set; }
		public IList<DateTime> timestamp { get; set; }
		public IList<SearchResult> searchResult { get; set; }
		public IList<PaginationOutput> paginationOutput { get; set; }
		public IList<string> itemSearchURL { get; set; }
		public IList<CategoryHistogramContainer> categoryHistogramContainer { get; set; }
	}

	public class FindItemsByKeywordsModel {
		public IList<FindItemsByKeywordsResponse> findItemsByKeywordsResponse { get; set; }
	}

	#endregion

}

#region Usings

using EBay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace EBay.Models {

	public partial class APIRequest {

		#region Model Attributes

		[Display(Name = "Acknowledgement")]
		public string Ack { get; set; }

		[Display(Name = "Keywords")]
		[Required(ErrorMessage = "Provide a few words that best describe your item")]
		[MinLength(2, ErrorMessage = "{0} must be at least {1} characters long")]
		public string Keywords { get; set; }

		[Display(Name = "Minimum Price")]
		public string MinPrice { get; set; }

		[Display(Name = "Maximum Price")]
		public string MaxPrice { get; set; }

		[Display(Name = "Listing Type")]
		public string ListingType { get; set; }

		[Display(Name = "Condition")]
		public string Condition { get; set; }

		[Display(Name = "Sort Order")]
		public string Sort { get; set; }

		[Display(Name = "Buying Format")]
		public string Format { get; set; }

		[Display(Name = "Delivery")]
		public string Delivery { get; set; }

		[Display(Name = "Location")]
		public string Location { get; set; }

		[Display(Name = "Sold Items")]
		public string Sold { get; set; }

		[Display(Name = "Completed Items")]
		public string Completed { get; set; }

		[Display(Name = "Free Shipping")]
		public string Shipping { get; set; }

		[Display(Name = "Returns Accepted")]
		public string Returns { get; set; }

		[Display(Name = "Page")]
		public int PageNumber { get; set; }

		[Display(Name = "Entries Per Page")]
		public int EntriesPerPage { get; set; }

		[Display(Name = "Total Entries")]
		public int TotalEntries { get; set; }

		[Display(Name = "Pages")]
		public int TotalPages { get; set; }

		[Display(Name = "Items")]
		public IEnumerable<Item> Items { get; set; }

		[Display(Name = "Error Message")]
		public string ErrorMessage { get; set; }

		#endregion

	}

}

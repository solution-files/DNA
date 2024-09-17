#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

	public partial class Cart {

		#region Model Attributes

		[Display(Name = "Cart")]
		public int CartId { get; set; }

		[Display(Name = "User")]
		public int? UserId { get; set; }

		[Display(Name = "Date")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yy}")]
		public DateTime? Date { get; set; }

		[Display(Name = "Status")]
		public int? StatusId { get; set; }

		#endregion

		#region Navigation Properties

		[Display(Name = "Status")]
		public Status Status { get; set; }

		[Display(Name = "Items")]
		public IList<Item> Items { get; set; }

		#endregion

	}

}

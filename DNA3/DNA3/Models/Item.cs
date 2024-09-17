#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

#nullable disable

namespace DNA3.Models {

	public partial class Item {

		#region Model Attributes

		[Display(Name = "Item")]
		public int ItemId { get; set; }

		[Display(Name = "Cart")]
		public int? CartId { get; set; }

		[Display(Name = "Product")]
		public int? ProductId { get; set; }

		[Display(Name = "Quantity")]
		public int? Quantity { get; set; }

		#endregion

		#region Navigation Properties

		[Display(Name = "Cart")]
		public Cart Cart { get; set; }

		[Display(Name = "Product")]
		public Product Product { get; set; }

		#endregion

	}

}

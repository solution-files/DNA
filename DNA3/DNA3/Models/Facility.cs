#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

	public partial class Facility {

		#region Model Attributes

		[Display(Name = "Facility")]
		public int FacilityId { get; set; }

		[Display(Name = "Code")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		public string Code { get; set; }

		[Display(Name = "Name")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		public string Name { get; set; }

		[Display(Name = "Address")]
		public string Address { get; set; }

		[Display(Name = "City")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		public string City { get; set; }

		[Display(Name = "State")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		public string State { get; set; }

		[Display(Name = "Zip")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		public string Zip { get; set; }

		[Display(Name = "Contact")]
		public string Contact { get; set; }

		[Display(Name = "Phone")]
		public string Phone { get; set; }

		[Display(Name = "Status")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		public int? StatusId { get; set; }

		#endregion

		#region Navigation Properties

		[Display(Name = "Status")]
		public Status Status { get; set; }

		#endregion

	}

}

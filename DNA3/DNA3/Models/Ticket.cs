#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

	public partial class Ticket {

		#region Model Attributes

		[Display(Name = "Ticket")]
		public int TicketId { get; set; }

		[Display(Name = "Client")]
		[Required(ErrorMessage = "{0} cannot be blank")]
		public int? ClientId { get; set; }

		[Display(Name = "Facility")]
		[Required(ErrorMessage = "{0} cannot be blank")]
		public int? FacilityId { get; set; }

		[Display(Name = "Task")]
		[Required(ErrorMessage = "{0} cannot be blank")]
		public int? TaskId { get; set; }

		[Display(Name = "Asset")]
		public int? AssetId { get; set; }

		[Display(Name = "Description")]
		public string Description { get; set; }

		[Display(Name = "Start")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yy hh:mm tt}")]
		[Required(ErrorMessage = "{0} cannot be blank")]
		public DateTime? Start { get; set; }

		[Display(Name = "Finish")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yy hh:mm tt}")]
		public DateTime? Finish { get; set; }

		#endregion

		#region Navigation Properties

		[Display(Name = "Client")]
		public Client Client { get; set; }

		[Display(Name = "Facility")]
		public Facility Facility { get; set; }

		[Display(Name = "Task")]
		public Task Task { get; set; }

		[Display(Name = "Asset")]
		public Asset Asset { get; set; }

		#endregion

	}

}

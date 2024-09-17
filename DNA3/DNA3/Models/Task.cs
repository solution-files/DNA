#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

	public partial class Task {

		#region Model Attributes

		[Display(Name = "Task")]
		public int TaskId { get; set; }

		[Display(Name = "Code")]
		public string Code { get; set; }

		[Display(Name = "Name")]
		public string Name { get; set; }

		[Display(Name = "Description")]
		public string Description { get; set; }

		[Display(Name = "Status")]
		public int? StatusId { get; set; }

		#endregion

		#region Navigation Properties

		[Display(Name = "Status")]
		public Status Status { get; set; }

		#endregion

	}

}


#region Usings

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

#nullable disable

namespace DNA3.Models {

	public partial class Databases {

		#region Model Attributes

		[Display(Name = "ID")]
		public int database_id { get; set; }

		[Display(Name = "Name")]
		public string name { get; set; }

		[Display(Name = "Access")]
		public string user_access_desc { get; set; }

		[Display(Name = "Created")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
		public DateTime create_date { get; set; }

		#endregion

	}

}

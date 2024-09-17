#region Usings

using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA.Models {

	public partial class Error {

		#region Model Attributes

		[Display(Name = "Error Code")]
		public int Code { get; set; }

		[Display(Name = "Name")]
		public string Name { get; set; }

		[Display(Name = "Description")]
		public string Description { get; set; }

		#endregion

	}

}

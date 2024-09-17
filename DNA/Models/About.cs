#region Usings

using DNA3.Models;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA.Models {

	public partial class About {

		#region Model Attributes

		[Display(Name = "Page")]
		public Page Page { get; set; }

		#endregion

	}

}

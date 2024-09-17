#region Usings

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

#nullable disable

namespace DNA3.Models {

	public partial class Asset {

		#region Model Attributes

		[Display(Name = "Asset")]
        public int AssetId { get; set; }

        [Display(Name = "Client")]
        public int? ClientId { get; set; }

        [Display(Name = "Name")]
		[Required(ErrorMessage = "{0} cannot be empty")]
        public string Name { get; set; }

        [Display(Name = "Description")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		public string Description { get; set; }

        [Display(Name = "Model Number")]
        public string Model { get; set; }

        [Display(Name = "Serial Number")]
        public string Serial { get; set; }

		[Display(Name = "Facility")]
		public int? FacilityId { get; set; }
		
		[Display(Name = "Area")]
        public string Area { get; set; }

		[Display(Name = "Vendor")]
		public int? VendorId { get; set; }
		
		[Display(Name = "Phone")]
        public string Phone { get; set; }

		#endregion

		#region Unmapped Attributes

		[NotMapped]
		[Display(Name = "Serial Number (Name)")]
		public string NameSerial => $"{Name} ({Serial})";

		#endregion

		#region Navigation Properties

		[Display(Name = "Client")]
		public Client Client { get; set; }

		[Display(Name = "Facility")]
		public Facility Facility { get; set; }

		[Display(Name = "Vendor")]
		public Vendor Vendor { get; set; }

        #endregion

    }

}

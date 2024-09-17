#region Usings

using System;
using System.ComponentModel.DataAnnotations;
using Utilities;

#endregion

#nullable disable

namespace DNA3.Models {
    public partial class Vendor {

        #region Variables

        string _Phone;

        #endregion

        #region Model Attributes

        [Display(Name = "Vendor")]
		public int VendorId { get; set; }

		[Display(Name = "Code")]
		[Required(ErrorMessage = "{0} cannot be blank")]
		public string Code { get; set; }

		[Display(Name = "Name")]
		[Required(ErrorMessage = "{0} cannot be blank")]
		public string Name { get; set; }

		[Display(Name = "Address")]
		public string Address { get; set; }

		[Display(Name = "City")]
		public string City { get; set; }

		[Display(Name = "State")]
		public string State { get; set; }

		[Display(Name = "Zip")]
		public string Zip { get; set; }

        [Display(Name = "Zip 4")]
        public string Zip4 { get; set; }
        
        [Display(Name = "Contact")]
		public string Contact { get; set; }

        [Display(Name = "Phone")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        [Phone(ErrorMessage = "Please specify a valid {0}")]
        public string Phone {
            get { return (String.IsNullOrEmpty(_Phone) ? "" : string.Format("{0:(###) ###-####}", long.Parse(Strings.NumericValue(_Phone)))); }
            set { _Phone = Strings.NumericValue(value); }
        }

        [Display(Name = "E-Mail Address")]
        [EmailAddress(ErrorMessage = "Please specify a valid {0}")]
        public string Email { get; set; }

        [Display(Name = "Comments")]
        public string Comment { get; set; }
        
        [Display(Name = "Status")]
		[Required(ErrorMessage = "{0} cannot be blank")]
		public int? StatusId { get; set; }

		#endregion

		#region Navigation Properties

        [Display(Name = "Status")]
		public Status Status { get; set; }

		#endregion

	}

}

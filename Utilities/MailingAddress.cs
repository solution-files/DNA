#region Usings

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace Utilities {

    public partial class MailingAddress {

        #region Model Attributes

        [Display(Name = "ID")]
        public int? Id { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "{0} cannot be blank.")]
        public string City { get; set; }

        [Display(Name = "County")]
        [Required(ErrorMessage = "{0} cannot be blank.")]
        public string County { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "{0} cannot be blank.")]
        public string State { get; set; }

        [Display(Name = "Zip")]
        public string Zip5 { get; set; }

        [Display(Name = "Plus4")]
        public string Zip4 { get; set; }

        #endregion

        #region Navigation Properties


        #endregion

    }

}

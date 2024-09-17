#region Usings

using System.ComponentModel.DataAnnotations;

#endregion

namespace DNA3.Models {

    public partial class Recover {

        #region Model Attributes

        [Display(Name = "E-Mail Address")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        [EmailAddress(ErrorMessage = "Please provide a valid {0}")]
        public string Email { get; set; }

        [Display(Name = "Provider")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Provider { get; set; }

        #endregion

    }

}

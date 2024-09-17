#region Usings

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

#endregion

namespace DNA3.Models {

    public partial class Registration {

        #region Model Attributes

        [Display(Name = "Client ID")]
        public int ClientId { get; set; }

        [Display(Name = "Provider")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Provider { get; set; }

        [Display(Name = "Company Name")]
		[Required(ErrorMessage = "{0} cannot be blank")]
		[MinLength(2, ErrorMessage = "{0} must be at least {1} characters long")]
		public string Company { get; set; }

        [Display(Name = "E-Mail Address")]
        [Required(ErrorMessage = "{0} cannot be blank")]
		[EmailAddress(ErrorMessage = "Please provide a complete {0}")]
        [Remote("RegistrationNotRegisteredUser", "Dashboard", HttpMethod = "GET", ErrorMessage = "You already have an account. Please sign-in.")]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
		[MinLength(2, ErrorMessage = "{0} must be at least {1} characters long")]
        public string First { get; set; }

        [Display(Name = "Last Name")]
		[Required(ErrorMessage = "{0} cannot be blank")]
		[MinLength(2, ErrorMessage = "{0} must be at least {1} characters long")]
        public string Last { get; set; }

        [Display(Name = "Terms")]
		public bool Terms { get; set; }

        #endregion

    }

}

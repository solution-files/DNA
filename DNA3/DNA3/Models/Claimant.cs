#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

#nullable disable

namespace DNA3.Models {

    public class Claimant {

        #region Model Attributes

        [Display(Name = "Provider")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Provider { get; set; }

        [Display(Name = "E-Mail Address")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        [EmailAddress(ErrorMessage = "Please provide a valid {0}")]
        public string Email { get; set; }

        [Display(Name = "Password")]
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "{0} cannot be blank")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool Persist { get; set; }

        [Display(Name = "Full Name")]
        public string Full { get; set; }

        [Display(Name = "First Name")]
        public string First { get; set; }

        [Display(Name = "Last Name")]
        public string Last { get; set; }

        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [Display(Name = "User")]
        public int UserId { get; set; }

        [Display(Name = "Login")]
        public int LoginId { get; set; }

        [Display(Name = "Role")]
        public string RoleCode { get; set; }

        [Display(Name = "Status")]
        public string StatusCode { get; set; }

        [Display(Name = "Status Message")]
        public string StatusMessage { get; set; }

        [Display(Name = "Token")]
        public string Token { get; set; }

        [Display(Name = "Token Date")]
        public DateTime? Tokendate { get; set; }

		[Display(Name = "Return Url")]
		public string ReturnUrl { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Client")]
        public Client Client { get; set; }

        [Display(Name = "User")]
        public User User { get; set; }

        [Display(Name = "Identity")]
        public Login Login { get; set; }

        #endregion

    }

}

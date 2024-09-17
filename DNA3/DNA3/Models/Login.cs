#region Usings

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace DNA3.Models {

    public partial class Login {

		#region Model Attributes

		[Display(Name = "Identity")]
		public int LoginId { get; set; }

		[Display(Name = "Provider")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        public string Provider { get; set; }

		[Display(Name = "User")]
		public int? UserId { get; set; }

		[Display(Name = "E-Mail Address")]
		public string Email { get; set; }

		[Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "User")]
		public User User { get; set; }

		#endregion

	}

}

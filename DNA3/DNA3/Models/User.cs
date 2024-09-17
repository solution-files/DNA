#region Usings

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

#nullable disable

namespace DNA3.Models {

	public partial class User {

		#region Model Attributes

		[Display(Name = "User")]
        public int UserId { get; set; }

        [Display(Name = "Client")]
        public int ClientId { get; set; }

		[Display(Name = "First Name")]
        [Required(ErrorMessage = "{0} cannot be blank.")]
        public string First { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "{0} cannot be blank.")]
        public string Last { get; set; }

		[NotMapped]
		public string FullName => $"{First} {Last}";

        [Display(Name = "Role")]
        [Required(ErrorMessage = "{0} cannot be blank.")]
        public int RoleId { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "{0} cannot be blank.")]
        public int StatusId { get; set; }

		[Display(Name = "Remember Me")]
        public bool? Persist { get; set; }

        [Display(Name = "Key")]
        public string TotpKey { get; set; }

        [Display(Name = "Manual Setup Key")]
        public string TotpManualSetup { get; set; }

        [Display(Name = "Device Name")]
        public string TotpDeviceName { get; set; }

        [Display(Name = "TOTP Code")]
        public string TotpCode { get; set; }

        [Display(Name = "Token")]
        public string Token { get; set; }

        [Display(Name = "Token Date")]
        public DateTime? TokenDate { get; set; }

		[Display(Name = "Thumbprint")]
		public string Thumbprint { get; set; }

		[Display(Name = "Certificate")]
		public string Certificate { get; set; }

		[Display(Name = "Comment")]
		public string Comment { get; set; }

		#endregion

		#region Navigation Properties

		[Display(Name = "Client")]
		public Client Client { get; set; }

		[Display(Name = "Role")]
        public Role Role { get; set; }

        [Display(Name = "Status")]
        public Status Status { get; set; }

        [Display(Name = "Identities")]
        [JsonIgnore]
        public IList<Login> Logins { get; set; }

        #endregion

    }

}

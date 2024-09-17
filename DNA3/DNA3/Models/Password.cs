#region "Usings"

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace DNA3.Models {

    public partial class Password {

        #region "Model Attributes"

        [NotMapped]
        [Display(Name = "Identity")]
        public int LoginId { get; set; }

        [NotMapped]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} cannot be empty")]
        [Remote(action: "ValidPasswordOptions", controller: "Identity")]
        public string Proposed { get; set; }

        [NotMapped]
        [Display(Name = "Confirmed Value")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} cannot be empty")]
        [Compare(nameof(Proposed), ErrorMessage = "The {1} and {0} do not match.")]
        public string Confirmed { get; set; }

        #endregion

    }

}

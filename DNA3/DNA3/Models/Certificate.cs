#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Certificate {

        #region Model Attributes

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Name { get; set; }

        #endregion

        #region Navigation Properties

        #endregion

    }

}

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

// #nullable disable

namespace DNA3.Models {
    public partial class Source {


        #region Model Attributes

        [Display(Name = "Source")]
        public int SourceId { get; set; }

        [Display(Name = "Code")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        #endregion

        #region Navigation Properties

        #endregion

    }

}

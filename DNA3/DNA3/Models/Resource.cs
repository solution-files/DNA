#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

// #nullable disable

namespace DNA3.Models {

    public partial class Resource {

        #region Model Attributes

        [Display(Name="Resource")]
        public int ResourceId { get; set; }

        [Display(Name = "Part Number")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public int? PartNumber { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Description { get; set; }

        [Display(Name = "Unit")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Unit { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public decimal? Price { get; set; }

        #endregion

    }

}

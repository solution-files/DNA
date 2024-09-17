#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Product {

        #region Model Attributes

        [Display(Name="Product")]
        public int ProductId { get; set; }

        [Display(Name = "Code")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        public string Description { get; set; }

        [Display(Name = "Features")]
        public string Features { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }

        [Display(Name = "Target")]
        public string Target { get; set; }

        [Display(Name = "Target Name")]
        public string TargetName { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal? Price { get; set; }

        [Display(Name = "Icon")]
        public string Icon { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        public int? StatusId { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Status")]
        public Status Status { get; set; }

        #endregion

    }

}

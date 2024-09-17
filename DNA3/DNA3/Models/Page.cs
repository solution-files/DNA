#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Page {

        #region Model Attributes

        [Display(Name = "Page")]
        public int PageId { get; set; }

        [Display(Name = "Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
		[Required(ErrorMessage = "{0} cannot be blank")]
        public DateTime? Date { get; set; }

        [Display(Name = "Slug")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Slug { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Name { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        [MinLength(6, ErrorMessage = "{0} must be at least {1} characters long")]
        public string Subject { get; set; }

        [Display(Name = "Content")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        [MinLength(6, ErrorMessage = "{0} must be at least {1} characters long")]
        public string Content { get; set; }

        [Display(Name = "Icon")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Icon { get; set; }

        #endregion

    }

}

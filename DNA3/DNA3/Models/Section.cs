#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Section {

        #region Model Attributes

        [Display(Name = "Section")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public int SectionId { get; set; }

        [Display(Name = "Page")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public int PageId { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Date { get; set; }

        [Display(Name = "Slug")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Slug { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Name { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Subject { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Description { get; set; }

        [Display(Name = "Icon")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Icon { get; set; }

        [Display(Name = "Columns")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public int? Columns { get; set; }

        [Display(Name = "Article Limit")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public int? Limit { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Page")]
        public Page Page { get; set; }

        #endregion

    }

}

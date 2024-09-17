#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Category {

        #region Model Attributes

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Section")]
        [Required(ErrorMessage = "Please select a {0} from the list")]
        public int SectionId { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Date { get; set; }

        [Display(Name = "Slug")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Slug { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please select a {0} from the list")]
        public string Name { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Please select a {0} from the list")]
        public string Subject { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Please select a {0} from the list")]
        public string Description { get; set; }

        [Display(Name = "Icon")]
        public string Icon { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Section")]
        public Section Section { get; set; }

        #endregion

    }

}

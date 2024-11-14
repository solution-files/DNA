#region Usings

using System;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Project {

        #region Model Attributes

        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        [MinLength(10, ErrorMessage = "{0} must be at least {1} characters long")]
        public string Subject { get; set; }

        [Display(Name = "Content")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        [MinLength(10, ErrorMessage = "{0} must be at least {1} characters long")]
        public string Content { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Please select a valid {0} from the list")]
        public int StatusId { get; set; }

        #endregion

        #region Navigation Properties (Not allowed in this Model. Use a separate View Model for reports, etc.)

        #endregion

    }

}

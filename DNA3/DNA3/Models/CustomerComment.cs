#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

// #nullable disable

namespace DNA3.Models {

    public partial class CustomerComment {

        #region Model Attributes

        [Display(Name = "Comment")]
        public int CustomerCommentId { get; set; }

        [Display(Name = "Customer")]
        [Required(ErrorMessage = "Please select an {0} from the list")]
        public int? CustomerId { get; set; }

        [Display(Name = "User")]
        [Required(ErrorMessage = "Please select a valid {0} from the list")]
        public int? UserId { get; set; }

        [Display(Name = "Comment Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy hh :mm tt}")]
        public DateTime? CommentDate { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Description { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Customer")]
        public virtual Customer Customer { get; set; }

        [Display(Name = "User")]
        public virtual User User { get; set; }

        #endregion

    }

}

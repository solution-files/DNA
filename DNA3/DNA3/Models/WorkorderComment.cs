#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#region Directives

// #nullable disable

#endregion

#region Class

namespace DNA3.Models {

    public partial class WorkorderComment {

        #region Model Attributes

        [Display(Name = "Comment")]
        public int WorkorderCommentId { get; set; }

        [Display(Name = "Workorder")]
        [Required(ErrorMessage = "Please select an {0} from the list")]
        public int? WorkorderId { get; set; }

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

        [Display(Name = "Workorder")]
        public virtual Workorder Workorder { get; set; }

        [Display(Name = "User")]
        public virtual User User { get; set; }

        #endregion

    }

}

#endregion
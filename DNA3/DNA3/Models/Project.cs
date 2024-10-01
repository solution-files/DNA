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
        public string Subject { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Status")]
        public Status Status { get; set; }

        #endregion

    }

}

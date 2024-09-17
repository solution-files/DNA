#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

// #nullable disable

namespace DNA3.Models {

    public partial class Note {

        #region Model Attributes

        [Display(Name="Note")]
        public int NoteId { get; set; }

        [Display(Name = "Customer")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public int? CustomerId { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Subject { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Description { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Customer")]
        public Customer Customer { get; set; }

        #endregion

    }

}

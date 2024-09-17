#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

	public partial class Status {

        #region Model Attributes

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Display(Name = "Table")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public int TableId { get; set; }

        [Display(Name = "Code")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Description { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Table Table { get; set; }

        #endregion

    }

}

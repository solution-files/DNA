#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace DNA3.Models {

    public partial class Condition {

        #region Model Attributes

        [Display(Name = "Condition")]
        public int ConditionId { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        #endregion

    }

}

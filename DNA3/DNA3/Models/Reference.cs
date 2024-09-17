#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Reference {

        #region Model Attributes

        [Display(Name="Reference")]
        public int ReferenceId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Target")]
        public string Target { get; set; }

        [Display(Name = "Target Name")]
        public string TargetName { get; set; }

        [Display(Name = "Icon")]
        public string Icon { get; set; }

        #endregion

    }

}

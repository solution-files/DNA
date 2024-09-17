#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Menu {

        #region Model Attributes

        [Display(Name = "Menu")]
        public int MenuId { get; set; }

        [Display(Name = "Role")]
        public int RoleId { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Name")]
		public string Name { get; set; }

        [Display(Name = "Description")]
		public string Description { get; set; }

        [Display(Name = "Icon")]
        public string Icon { get; set; }

        [Display(Name = "Target")]
        public string Target { get; set; }

        [Display(Name = "Target Name")]
        public string TargetName { get; set; }

        [Display(Name = "Top Level")]
        public bool TopLevel { get; set; }

        [Display(Name = "Weight")]
		public int? Weight { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Role")]
        public Role Role { get; set; }

        [Display(Name = "Actions")]
        public IList<Action> Actions { get; set; }

        #endregion

    }

}

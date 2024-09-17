#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Action {

        #region Model Attributes

        [Display(Name = "Action")]
        public int ActionId { get; set; }

        [Display(Name = "Menu")]
        public int? MenuId { get; set; }

        [Display(Name = "Role")]
        public int? RoleId { get; set; }

        [Display(Name = "Code")]
		[Required(ErrorMessage = "{0} cannot be empty")]
        public string Code { get; set; }

        [Display(Name = "Name")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Icon")]
        public string Icon { get; set; }

        [Display(Name = "CSS Class")]
        public string Class { get; set; }

        [Display(Name = "Target")]
        public string Target { get; set; }

        [Display(Name = "Target Name")]
        public string TargetName { get; set; }

        [Display(Name = "Weight")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		public int? Weight { get; set; }

		[Display(Name = "New Window")]
		public bool NewWindow { get; set; }
		
		#endregion

		#region Navigation Properties

		[Display(Name = "Menu")]
        public Menu Menu { get; set; }

        [Display(Name = "Role")]
        public Role Role { get; set; }

        #endregion

    }

}

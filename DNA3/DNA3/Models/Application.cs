#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable enable

namespace DNA3.Models {

    public partial class Application {

        #region Constructor

        public Application() {
            Icon = "";
            Type = "";
            Code = "";
            Name = "";
            Description = "";
            Target = "";
            TargetName = "";
            Area = "";
            Controller = "";
            Action = "";
            Role = new();
        }

        #endregion

        #region Model Attributes

        [Display(Name = "Application")]
        public int ApplicationId { get; set; }

        [Display(Name = "Menu")]
        public int? MenuId { get; set; }

        [Display(Name = "Role")]
        public int RoleId { get; set; }
        
        [Display(Name = "Icon")]
		public string Icon { get; set; }

		[Display(Name = "Type")]
		[Required(ErrorMessage = "{0} cannot be empty")]
		public string Type { get; set; }

		[Display(Name = "Code")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        public string Description { get; set; }

        [Display(Name = "Target")]
        public string Target { get; set; }

		[Display(Name = "Target Name")]
		public string TargetName { get; set; }

		[Display(Name = "Area")]
		public string Area { get; set; }

		[Display(Name = "Controller")]
		public string Controller { get; set; }

		[Display(Name = "Action")]
		public string Action { get; set; }

		[Display(Name = "Weight")]
        [Required(ErrorMessage = "{0} cannot be blank.")]
        [Range(0, Int32.MaxValue, ErrorMessage = "{0} must be greater than {1}")]
        public int? Weight { get; set; }

		#endregion

		#region Navigation Properties

		[Display(Name = "Menu")]
		public Menu? Menu { get; set; }

        [Display(Name = "Role")]
        public Role Role { get; set; }
        
        #endregion

    }

}

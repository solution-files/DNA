#region Usings

using System;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {
    public partial class Service {

        #region Model Attributes

        [Display(Name = "Service")]
        public int ServiceId { get; set; }

        [Display(Name = "Code")]
        [Required(ErrorMessage = "{0} canot be blank")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} canot be blank")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} canot be blank")]
        public string Description { get; set; }

        [Display(Name = "Port")]
        public int Port { get; set; }

        [Display(Name = "Protocol")]
        public string Protocol { get; set; }

        #endregion

        #region Navigation Properties



        #endregion

    }

}

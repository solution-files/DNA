#region Usings

using System;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {
    public partial class Deviceservice {

        #region Model Attributes

        [Display(Name = "Identity")]
        public int DeviceserviceId { get; set; }

        [Display(Name = "Device")]
        [Required(ErrorMessage = "{0} canot be blank")]
        public int? DeviceId { get; set; }

        [Display(Name = "Service")]
        [Required(ErrorMessage = "{0} canot be blank")]
        public int? ServiceId { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Device")]
        public Device Device { get; set; }

        [Display(Name = "Service")]
        public Service Service { get; set; }

        #endregion

    }

}

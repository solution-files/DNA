#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {
    public partial class Device {

        #region Model Attributes

        [Display(Name = "Device")]
        public int DeviceId { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Hostname")]
        [Required(ErrorMessage = "{0} canot be blank")]
        public string Hostname { get; set; }

        [Display(Name = "IPV4")]
        [Required(ErrorMessage = "{0} canot be blank")]
        public string Ipv4 { get; set; }

        [Display(Name = "IPV4 Gateway")]
        public string Ipv4Gateway { get; set; }

        [Display(Name = "Public IPV4")]
        public string Ipv4Public { get; set; }

        [Display(Name = "Connected")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd HH:mm:ss}")]
        public DateTime? Connected { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        #endregion

        #region Navigation Properties



        #endregion

    }

}

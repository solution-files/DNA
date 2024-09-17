#region Usings

using System;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace Utilities.Models {
    public partial class Device {

        #region Model Attributes

        [Display(Name = "Device")]
        public int DeviceId { get; set; }

        [Display(Name = "Hostname")]
        public string Hostname { get; set; }

        [Display(Name = "IPV4")]
        public string Ipv4 { get; set; }

        [Display(Name = "IPV4 Gateway")]
        public string Ipv4Gateway { get; set; }

        [Display(Name = "Public IPV4")]
        public string Ipv4Public { get; set; }

        [Display(Name = "Connected")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd HH:mm}")]
        public DateTime? Connected { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        #endregion

    }

}

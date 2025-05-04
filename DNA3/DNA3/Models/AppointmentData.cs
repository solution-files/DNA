#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class AppointmentData {

        #region Model Attributes

        [Display(Name = "Appointment")]
        public int Id { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Start")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End")]
        public DateTime EndTime { get; set; }

        #endregion

        #region Navigation Properties



        #endregion

    }

}

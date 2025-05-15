#region Usings

using Newtonsoft.Json;
using System;
using DNA3.Models;

#endregion

namespace DNA3.Models {

    #region Class

    public class Appointment {

        #region Model Attributes

        public int AppointmentId { get; set; }

        public int AppointmentTypeId { get; set; }

        public DateTime StartTime { get; set; }

        public string StartTimeZone { get; set; }

        public DateTime EndTime { get; set; }

        public string EndTimeZone { get; set; }

        public string Subject { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public bool AllDay { get; set; }

        public bool Recurrence { get; set; }

        public string RecurrenceRule { get; set; }

        #endregion

        #region Navigation Properties

        public AppointmentType AppointmentType;

        #endregion

    }

    #endregion

}

#region Usings

using DNA3.Models;

#endregion

#region Directives

#nullable disable

#endregion

namespace Schedule.Models {
    public class EditParams {

        #region Model Attributes
        public string key { get; set; }
        public string action { get; set; }
        public List<Appointment> added { get; set; }
        public List<Appointment> changed { get; set; }
        public List<Appointment> deleted { get; set; }
        public Appointment value { get; set; }

        #endregion

    }

}

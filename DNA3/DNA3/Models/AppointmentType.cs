#region Usings

using Newtonsoft.Json;
using System;
using DNA3.Models;

#endregion

namespace DNA3.Models {

    #region Class

    public class AppointmentType {

        #region Model Attributes

        public int AppointmentTypeId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        #endregion

        #region Navigation Properties



        #endregion

    }

    #endregion

}

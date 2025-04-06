#region Usings

using VIN.Models;

#endregion

#nullable disable

namespace VIN.Models {

    public class TrackingParams {

        #region Model Attributes

        public string idFromProvider { get; set; }
        public string remoteDealerId { get; set; }
        public string dealerName { get; set; }
        public string remoteSku { get; set; }
        public string experience { get; set; }
        public object rooftopUniqueName { get; set; }
        public object rooftopUuid { get; set; }
        public string dealerUniqueName { get; set; }
        public string dealerUuid { get; set; }
        public string dealerGroupUniqueName { get; set; }
        public string dealerGroupUuid { get; set; }

        #endregion

    }

}

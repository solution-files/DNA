#region Usings

using VIN.Models;

#endregion

#nullable disable

namespace VIN.Models {

    public class Root {

        #region Model Attributes

        public int totalCount { get; set; }
        public string totalCountFormatted { get; set; }
        public int hitsCount { get; set; }
        public List<Record> records { get; set; }
        public List<object> promotedAggregations { get; set; }

        #endregion

    }

}

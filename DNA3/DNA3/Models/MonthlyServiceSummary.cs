#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

// #nullable disable

namespace DNA3.Models {

    public partial class MonthlyServiceSummary {

        #region Model Attributes

        [Display(Name = "Year")]
        public int? Year { get; set; }

        [Display(Name = "Month")]
        public string Month { get; set; }

        [Display(Name = "Labor")]
        [DataType(DataType.Currency)]
        public decimal? Labor { get; set; }

        [Display(Name = "Parts")]
        [DataType(DataType.Currency)]
        public decimal? Parts { get; set; }

        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public decimal? Total { get; set; }

        #endregion

    }

}

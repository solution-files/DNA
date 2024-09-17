#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

// #nullable disable

namespace DNA3.Models {

    public partial class History {

        #region Model Attributes

        [Display(Name="Entry")]
        public int HistoryId { get; set; }

        [Display(Name = "Customer")]
        public int? CustomerId { get; set; }

        [Display(Name = "Disposition")]
        public int? DispositionId { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:MM/dd/yyyy}")]
        public DateTime? Date { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        [Display(Name = "Cycle Time")]
        public string Cycle { get; set; }

        [Display(Name = "Salt In Tank")]
        public int? Saltunit { get; set; }

        [Display(Name = "Salt Level")]
        public decimal? Saltlevel { get; set; }

        [Display(Name = "Hardness")]
        public int? Hardness { get; set; }

        [Display(Name = "Capacity")]
        public int? Capacity { get; set; }

        [Display(Name = "Parts")]
        [DataType(DataType.Currency)]
        public decimal? Parts { get; set; }

        [Display(Name = "Labor")]
        [DataType(DataType.Currency)]
        public decimal? Labor { get; set; }

        [Display(Name = "Tax")]
        [DataType(DataType.Currency)]
        public decimal? Tax { get; set; }

        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public decimal? Total {
            get { return Parts + Labor + Tax; }
            set { }
        }

        #endregion

        #region Navigation Properties

        [Display(Name = "Customer")]
        public virtual Customer Customer { get; set; }

        [Display(Name = "Disposition")]
        public virtual Disposition Disposition { get; set; }

        #endregion

    }

}

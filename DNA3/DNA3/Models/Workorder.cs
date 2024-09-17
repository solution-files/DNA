#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

// #nullable disable

namespace DNA3.Models {

    public partial class Workorder {

        #region Model Attributes

        [Display(Name = "Workorder")]
        public int WorkorderId { get; set; }

        [Display(Name = "Customer")]
        [Required(ErrorMessage = "Please select an {0} from the list")]
        public int? CustomerId { get; set; }

        [Display(Name = "Associate")]
        [Required(ErrorMessage = "Please select an {0} from the list")]
        public int? AssociateId { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy hh tt}")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy hh tt}")]
        public DateTime? EndDate { get; set; }

        [NotMapped]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}")]
        public DateTime? ScheduleDate {
            get { return StartDate; }
        }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Subject { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Description { get; set; }

        [Display(Name = "Labor")]
        [DataType(DataType.Currency)]
        public decimal? Labor { get; set; }

        [Display(Name = "Parts")]
        [DataType(DataType.Currency)]
        public decimal? Parts { get; set; }

        [Display(Name = "Other")]
        [DataType(DataType.Currency)]
        public decimal? Other { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Please select an {0} from the list")]
        public string Type { get; set; }

        [Display(Name = "Payment")]
        public string Payment { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Please select an {0} from the list")]
        public string Status { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Customer")]
        public virtual Customer Customer { get; set; }

        [Display(Name = "Associate")]
        public virtual Associate Associate { get; set; }

        #endregion

    }

}

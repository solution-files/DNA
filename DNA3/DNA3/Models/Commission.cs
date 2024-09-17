#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

// #nullable disable

namespace DNA3.Models {

    public partial class Commission {

        #region Model Attributes

        [Display(Name = "Commission")]
        public int CommissionId { get; set; }

        [Display(Name = "Customer")]
        public int? CustomerId { get; set; }

        [Display(Name = "Associate")]
        public int? AssociateId { get; set; }

        [Display(Name = "Amount")]
        public decimal? Amount { get; set; }

        [Display(Name = "Percentage")]
        public decimal? Percentage { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Customer")]
        public virtual Customer Customer { get; set; }

        [Display(Name = "Associate")]
        public virtual Associate Associate { get; set; }

        #endregion

    }

}

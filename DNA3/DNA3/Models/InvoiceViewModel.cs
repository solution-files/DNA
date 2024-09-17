#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class InvoiceViewModel {

        #region Model Attributes

        [Display(Name = "Invoice")]
        public int InvoiceId { get; set; }

        [Display(Name = "Customer")]
        public int? CustomerId { get; set; }

        [Display(Name = "Date")]
        public DateTime? Date { get; set; }

        [Display(Name = "Code")]
        public int? Code { get; set; }

        [Display(Name = "Reference")]
        public string Reference { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Display(Name = "Status")]
        public int? StatusId { get; set; }

        #endregion

        #region Collections

        [Display(Name = "Customer")]
        public Customer Customer { get; set; }

        [Display(Name = "Status")]
        public Status Status { get; set; }

        [Display(Name = "Items")]
        public HashSet<InvoiceItem> Items { get; set; }

        #endregion

    }

}

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class InvoiceItem {

        #region Model Attributes

        [Display(Name = "Item")]
        public int InvoiceItemId { get; set; }

        [Display(Name = "Invoice")]
        public int? InvoiceId { get; set; }

        [Display(Name = "Product")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        public int? ProductId { get; set; }

        [Display(Name = "Quantity")]
        [Range(1, 99, ErrorMessage = "{0} must fall between {1} and {2}")]
        public decimal? Quantity { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal? Price { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Invoice")]
        public Invoice Invoice { get; set; }

        [Display(Name = "Product")]
        public Product Product { get; set; }

        #endregion

    }

}

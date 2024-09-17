#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities;

#endregion

//#nullable disable

namespace DNA3.Models {

    public partial class Associate {

        #region Variables

        private string _Phone = "";

        #endregion

        #region Model Attributes

        [Display(Name = "Associate")]
        public int AssociateId { get; set; }

        [Display(Name = "Code")]
        public int? Code { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        public string First { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "{0} cannot be empty")]
        public string Last { get; set; }

        [Display(Name = "Full Name")]
        [NotMapped]
        public string FullName => $"{First} {Last}";

        [Display(Name = "Full Name")]
        [NotMapped]
        public string LastFirst => $"{Last} {First}";

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public string Zip { get; set; }

        [Display(Name = "Plus 4")]
        public string Plus4 { get; set; }

        [Display(Name = "Phone")]
        public string Phone { 
            get { return (String.IsNullOrEmpty(_Phone) ? "" : string.Format("{0:(###) ###-####}", long.Parse(Strings.NumericValue(_Phone)))); } 
            set { _Phone = Strings.NumericValue(value); } 
        }

        [Display(Name = "E-Mail Address")]
        [EmailAddress(ErrorMessage = "Please supply a valid {0}")]
        public string Email { get; set; }

        [Display(Name = "Owner")]
        public bool Owner { get; set; }

        [Display(Name = "Setter")]
        public bool Setter { get; set; }

        [Display(Name = "Verifier")]
        public bool Verifier { get; set; }

        [Display(Name = "Confirmer")]
        public bool Confirmer { get; set; }

        [Display(Name = "Promoter")]
        public bool Promoter { get; set; }

        [Display(Name = "Sales Rep")]
        public bool Salesrep { get; set; }

        [Display(Name = "Service Tech")]
        public bool Servicetech { get; set; }

        [Display(Name = "Dialer")]
        public bool Dialer { get; set; }

        [Display(Name = "Exclude From Requests")]
        public bool Exclude { get; set; }

        [Display(Name = "Comments")]
        [MinLength(10, ErrorMessage = "{0} must consist of at least {1} characters")]
        public string Comments { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Status")]
        public Status Status { get; set; }

        #endregion

    }
}

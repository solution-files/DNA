#region Usings

using DNA3.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities;

#endregion

namespace DNA3.Models {

    public partial class Customer {

        #region Variables

        private string _Phone1;
        private string _Phone2;

        #endregion

        #region Model Attributes

        [Display(Name = "ID")]
        public int CustomerId { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Company")]
        public string Company { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string First1 { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Last1 { get; set; }

        [Display(Name = "Full Name")]
        [NotMapped]
        public string FullName1 => $"{First1} {Last1}";

        [Display(Name = "Full Name")]
        [NotMapped]
        public string LastFirst1 => $"{Last1} {First1}";

        [Display(Name = "Phone")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        [Phone(ErrorMessage = "Please specify a valid {0}")]
        public string Phone1 {
            get { return (String.IsNullOrEmpty(_Phone1) ? "" : string.Format("{0:(###) ###-####}", long.Parse(Strings.NumericValue(_Phone1)))); }
            set { _Phone1 = Strings.NumericValue(value); }
        }

        [Display(Name = "Exclude")]
        public bool Phone1Exclude { get; set; }

        [Display(Name = "First Name")]
        public string First2 { get; set; }

        [Display(Name = "Last Name")]
        public string Last2 { get; set; }

        [Display(Name = "Full Name")]
        [NotMapped]
        public string FullName2 => $"{First2} {Last2}";

        [Display(Name = "Full Name")]
        [NotMapped]
        public string LastFirst2 => $"{Last2} {First2}";

        [Display(Name = "Phone")]
        public string Phone2 {
            get { return (String.IsNullOrEmpty(_Phone2) ? "" : string.Format("{0:(###) ###-####}", long.Parse(Strings.NumericValue(_Phone2)))); }
            set { _Phone2 = Strings.NumericValue(value); }
        }

        [Display(Name = "Exclude")]
        public bool Phone2Exclude { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Display(Name = "Plus4")]
        public string Plus4 { get; set; }

        [Display(Name = "E-Mail Address")]
        [EmailAddress(ErrorMessage = "Please specify a valid {0}")]
        public string Email { get; set; }

        [Display(Name = "Recorded")]
        public DateTime? Recorded { get; set; }

        [Display(Name = "Installed")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Installed { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        [Display(Name = "Associate")]
        public int? AssociateId { get; set; }

        [Display(Name = "Campaign")]
        public int? CampaignId { get; set; }

        [Display(Name = "Source")]
        public int? SourceId { get; set; }

        [Display(Name = "Promotion")]
        public int? PromotionId { get; set; }

        [Display(Name = "Promotion Identity")]
        public string PromoIdentity { get; set; }

        [Display(Name = "Promotion Filled")]
        public int? PromoFilled { get; set; }

        [Display(Name = "High Risk")]
        public string HighRisk { get; set; }

        [Display(Name = "Equipment")]
        [CustomEquipment(ErrorMessage = "{0} must consist of AIR, AIRMASTER, AMC, AQC, CS, EC4, EC5, F8C, IRON, OXY, P-6, P-12, PRO, Q2, QRS, RO, TC, TCM, or TWIN")]
        public string Equipment { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? Price { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Associate")]
        public Associate Associate { get; set; }

        [Display(Name = "Campaign")]
        public Campaign Campaign { get; set; }

        [Display(Name = "Source")]
        public Source Source { get; set; }

        [Display(Name = "Promotion")]
        public Promotion Promotion { get; set; }

        #endregion


    }

}

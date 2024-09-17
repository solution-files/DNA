#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

// #nullable disable

namespace DNA3.Models {

    public partial class Material {

        #region Model Attributes

        [Display(Name = "Material")]
        public int MaterialId { get; set; }

        [Display(Name = "Full Name 1")]
        public string Name1 { get; set; }

        [Display(Name = "First Name 1")]
        public string First1 { get; set; }

        [Display(Name = "Last Name 1")]
        public string Last1 { get; set; }

        [Display(Name = "Full Name 2")]
        public string Name2 { get; set; }

        [Display(Name = "First Name 2")]
        public string First2 { get; set; }

        [Display(Name = "Last Name 2")]
        public string Last2 { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Display(Name = "Plus 4")]
        public string Plus4 { get; set; }

        [Display(Name = "Carrier")]
        public string Carrier { get; set; }

        [Display(Name = "County")]
        public string County { get; set; }

        [Display(Name = "Recorded")]
        public DateTime? Recorded { get; set; }

        [Display(Name = "Published")]
        public DateTime? Published { get; set; }

        [Display(Name = "Home Value")]
        public decimal? Value { get; set; }

        [Display(Name = "Phone 1")]
        public string Phone1 { get; set; }

        [Display(Name = "Phone 2")]
        public string Phone2 { get; set; }

        [Display(Name = "Associate")]
        public int AssociateId { get; set; }

        [Display(Name = "Campaign")]
        public int CampaignId { get; set; }

        [Display(Name = "Source")]
        public int SourceId { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Associate")]
        public virtual Associate Associate { get; set; }

        [Display(Name = "Campaign")]
        public virtual Campaign Campaign { get; set; }

        [Display(Name = "Source")]
        public virtual Source Source { get; set; }

        #endregion

    }

}

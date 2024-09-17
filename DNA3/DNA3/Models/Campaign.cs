#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {
    public partial class Campaign {

        #region Model Attributes

        [Display(Name = "Campaign")]
        public int CampaignId { get; set; }

        [Display(Name = "Icon")]
        public string Icon { get; set; }

        [Display(Name = "Code")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        #endregion

        #region Navigation Properties


        #endregion

    }

}

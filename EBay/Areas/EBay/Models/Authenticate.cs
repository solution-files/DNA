#region Usings

using DNA3.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace EBay.Models {

    public class Authenticate {

        #region Model Attributes

        [Display(Name = "Application Token")]
        public int AppToken { get; set; }

        [Display(Name = "User Access Token")]
        public string UserToken { get; set; }

        #endregion

        #region Navigation Properties

        #endregion

    }

}

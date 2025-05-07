#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Gemini {

        #region Model Attributes

        [Display(Name = "Prompt")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string prompt { get; set; }

        #endregion

    }

}

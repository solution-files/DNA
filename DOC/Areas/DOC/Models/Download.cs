#region Usings

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

#nullable disable

namespace DOC.Models {

    public partial class Download {

        #region Model Attributes

        [NotMapped]
        [Display(Name = "File Name")]
        public string FileName { get; set; }

        #endregion

        #region Navigation Properties


        #endregion

    }

}

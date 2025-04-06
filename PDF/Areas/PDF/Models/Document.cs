#region Usings

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

#nullable enable

namespace PDF.Models {

    public partial class Document {

        #region Model Attributes

        [NotMapped]
        [Display(Name = "File Name")]
        public string? FileName { get; set; }

        #endregion

        #region Navigation Properties


        #endregion

    }

}

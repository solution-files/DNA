#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DOC.Models {

    public partial class Backup {

        #region Model Attributes

        [Display(Name = "Directories")]
        public int Directories { get; set; }

        [Display(Name = "Files")]
        public int Files { get; set; }

        [Display(Name = "Size (Uncompressed)")]
        public string Uncompressed { get; set; }

        [Display(Name = "Size (Compressed)")]
        public string Compressed { get; set; }

        #endregion

    }

}

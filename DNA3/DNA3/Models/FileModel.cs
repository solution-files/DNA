#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace DNA3.Models {
    public class FileModel {

        #region Model Attributes

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        #endregion

    }
}

#region Usings

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

// #nullable disable

namespace DNA3.Models {
    public partial class Table {

        #region Attributes

        [Display(Name = "Table")]
        public int TableId { get; set; }

        [Display(Name = "Code")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Description { get; set; }

        #endregion

        #region Navigation Properties

        #endregion

    }

}

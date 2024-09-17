#region Usings

using System;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Article {

        #region Model Attributes

        [Display(Name = "Article")]
        public int ArticleId { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Date { get; set; }

        [Display(Name = "Page")]
        [Required(ErrorMessage = "Please select a {0} from the list")]
        public int? PageId { get; set; }

        [Display(Name = "Section")]
        [Required(ErrorMessage = "Please select a {0} from the list")]
        public int? SectionId { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please select a {0} from the list")]
        public int? CategoryId { get; set; }

        [Display(Name = "Icon")]
        public string Icon { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Name { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        [MinLength(7, ErrorMessage = "{0} must be at least {1} characters")]
        public string Subject { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        [MinLength(10, ErrorMessage = "{0} must be at least {1} characters")]
        public string Description { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }

        [Display(Name = "Target")]
        public string Target { get; set; }

        [Display(Name = "Target Name")]
        public string TargetName { get; set; }

        [Display(Name = "Tags")]
        public string Tags { get; set; }

        [Display(Name = "Weight")]
        public int? Weight { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Please select a {0} from the list")]
        public int? StatusId { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Page")]
        public Page Page { get; set; }

        [Display(Name = "Section")]
        public Section Section { get; set; }

        [Display(Name = "Category")]
        public Category Category { get; set; }

        [Display(Name = "Status")]
        public Status Status { get; set; }

        #endregion

    }

}

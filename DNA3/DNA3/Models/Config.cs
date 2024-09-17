#region Usings

using System.ComponentModel.DataAnnotations;
using DNA3.CustomValidators;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Config {

        #region Model Attributes

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Name { get; set; }

        [Display(Name = "Short Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string ShortName { get; set; }

        [Display(Name = "Version")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Version { get; set; }

        [Display(Name = "Author")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Author { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Description { get; set; }

        [Display(Name = "Keywords")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Keywords { get; set; }

        [Display(Name = "Tagline")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Tagline { get; set; }

        [Display(Name = "Copyright")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Copyright { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Company { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Address { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string State { get; set; }

        [Display(Name = "Zip")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Zip { get; set; }

        [Display(Name = "Phone")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        [Phone(ErrorMessage = "Please spcify a valid {0}")]
        public string Phone { get; set; }

        [Display(Name = "Theme Name")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Themename { get; set; }

        [Display(Name = "Theme Path")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Themepath { get; set; }

        [Display(Name = "Apple")]
        public string Apple { get; set; }

        [Display(Name = "Facebook")]
        public string Facebook { get; set; }

        [Display(Name = "GitHub")]
        public string GitHub { get; set; }

        [Display(Name = "Google")]
        public string Google { get; set; }

        [Display(Name = "Instagram")]
        public string InstaGram { get; set; }

        [Display(Name = "LinkedIn")]
        public string LinkedIn { get; set; }

        [Display(Name = "Mastodon")]
        public string Mastodon { get; set; }

        [Display(Name = "Microsoft")]
        public string Microsoft { get; set; }

        [Display(Name = "Twitter")]
        public string Twitter { get; set; }

        [Display(Name = "Xing")]
        public string Xing { get; set; }

        #endregion

    }

}

#region Usings

using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace DNA3.Models {

    #region AppSettings

    // AppSettings object = JsonConvert.DeserializeObject<AppSettings>(jsoncontent);
    public class AppSettingsGrr {

        [Display(Name = "Application")]
        public App App { get; set; }

        [Display(Name = "Allowed Hosts")]
        public string AllowedHosts { get; set; }
    }

    #endregion

    #region App

    public class App {

        [Display(Name = "Application Name")]
        public string Name { get; set; }

        [Display(Name = "Short Name")]
        public string Shortname { get; set; }

        [Display(Name = "Application Version")]
        public string Version { get; set; }

        [Display(Name = "Domain Name")]
        public string Domain { get; set; }

        [Display(Name = "Application URL")]
        public string URL { get; set; }

        [Display(Name = "Application Keywords")]
        public string Keywords { get; set; }

        [Display(Name = "Application Tagline")]
        public string Tagline { get; set; }

        [Display(Name = "Application Description")]
        public string Description { get; set; }

        [Display(Name = "Application Copyright")]
        public string Copyright { get; set; }

        [Display(Name = "Company Name")]
        public string Company { get; set; }

        [Display(Name = "Street Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public string Zip { get; set; }

        [Display(Name = "Telephone Number")]
        public string Phone { get; set; }

        [Display(Name = "Application Author")]
        public string Author { get; set; }

        [Display(Name = "Billing Name")]
        public string BillingName { get; set; }

        [Display(Name = "Billing E-Mail Address")]
        public string BillingAddress { get; set; }

        [Display(Name = "Sales Name")]
        public string SalesName { get; set; }

        [Display(Name = "Sales E-Mail Address")]
        public string SalesAddress { get; set; }

        [Display(Name = "Support Name")]
        public string SupportName { get; set; }

        [Display(Name = "Support E-Mail Address")]
        public string SupportAddress { get; set; }

        [Display(Name = "Apple Link Address")]
        public string Apple { get; set; }

        [Display(Name = "Facebook Link Address")]
        public string Facebook { get; set; }

        [Display(Name = "GetHub Length")]
        public string Github { get; set; }

        [Display(Name = "Required Link Address")]
        public string Google { get; set; }

        [Display(Name = "Instagram Link Address")]
        public string Instagram { get; set; }

        [Display(Name = "LinkedIn Link Address")]
        public string Linkedin { get; set; }

        [Display(Name = "Mastodon Link Address")]
        public string Mastodon { get; set; }

        [Display(Name = "Microsoft Link Address")]
        public string Microsoft { get; set; }

        [Display(Name = "Twitter Link Address")]
        public string Twitter { get; set; }

        [Display(Name = "Xing Link Address")]
        public string Xing { get; set; }

        [Display(Name = "Theme Name")]
        public string Themename { get; set; }

        [Display(Name = "Theme Path")]
        public string Themepath { get; set; }
    }

    #endregion

}

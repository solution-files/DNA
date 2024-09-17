#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Client {

        #region Variables

        private string _Phone;

        #endregion

        #region Model Attributes

        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [Display(Name = "Company")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Company { get; set; }

        [Display(Name = "Address Line 1")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Address1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string Address2 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public string Zip { get; set; }

        [Display(Name = "Zip 4")]
        public string Zip1 { get; set; }

        [Display(Name = "First Name")]
        public string First { get; set; }

        [Display(Name = "Last Name")]
        public string Last { get; set; }

        [Display(Name = "Telephone")]
        public string Phone {
            get { return (String.IsNullOrEmpty(_Phone) ? "" : string.Format("{0:(###) ###-####}", long.Parse(Strings.NumericValue(_Phone)))); }
            set { _Phone = value; }
        }

        [Display(Name = "E-Mail Address")]
        [EmailAddress(ErrorMessage = "Please specify a valid {0}")]
        public string Email { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Display(Name = "Avitar")]
        public string Avitar { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Please select a valid {0} from the list")]
        public int? StatusId { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = "Status")]
        public Status Status { get; set; }

        [Display(Name = "Users")]
        [JsonIgnore]
        public ICollection<User> Users { get; set; }

        #endregion

    }

}

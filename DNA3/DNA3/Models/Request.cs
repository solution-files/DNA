#region Usings

using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Request {

        #region Model Attributes

        [Display(Name = "Request")]
        public int RequestId { get; set; }

        [Display(Name = "Date")]
        public DateTime? Date { get; set; }

        [Display(Name = "Company Name")]
        public string Company { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "(Cannot be empty)")]
        public string First { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "(Cannot be empty)")]
        public string Last { get; set; }

        [Display(Name = "E-Mail Address")]
        [Required(ErrorMessage = "(Cannot be empty)")]
        [EmailAddress(ErrorMessage = "(Must be a valid address)")]
        [Remote("RequestNotRegisteredUser", "Dashboard", ErrorMessage = "(Please use the support system)")]
        public string Email { get; set; }

        [Display(Name = "Message Subject")]
        [Required(ErrorMessage = "(Cannot be empty)")]
        [MinLength(10, ErrorMessage ="{0} must be at least {1} characters long")]
        public string Subject { get; set; }

        [Display(Name = "Message Content")]
        [Required(ErrorMessage = "(Cannot be empty)")]
        [MinLength(10, ErrorMessage = "(Must be at least {1} characters)")]
        public string Content { get; set; }

        [Display(Name = "Subscribe to our Newsletter")]
        public string Subscribe { get; set; }

        [Display(Name = "Request Type")]
        public string Type { get; set; }

        #endregion

    }

}

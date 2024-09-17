#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace DNA3.Models {

    public partial class Activity {

        #region "Model Attributes"

        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        [Display(Name = "Message Template")]
        public string MessageTemplate { get; set; }

        [Display(Name = "Level")]
        public string Level { get; set; }

        [Display(Name = "Time Stamp")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yy HH:mm:ss}")]
        public DateTime TimeStamp { get; set; }

        [Display(Name = "Exception")]
        public string Exception { get; set; }

        [Display(Name = "User ID")]
        public int? UserId { get; set; }

        #endregion

        #region Navigation Properties

		public User User { get; set; }

        #endregion

    }

}

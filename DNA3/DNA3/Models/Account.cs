#region Usings

using System;
using System.Collections.Generic;

#endregion

#nullable disable

namespace DNA3.Models {

    public partial class Account {

        #region Model Attributes

        public int AccountId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        #endregion

        #region Navigation Properties

        #endregion

    }

}

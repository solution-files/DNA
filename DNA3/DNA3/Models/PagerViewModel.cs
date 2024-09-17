#region Usings

using System;
using System.Linq;

#endregion

namespace DNA3.Models {

    public class PagerViewModel {

        #region Model Attributes

        public PagerViewModel() {
            Numeric = true;
            Info = true;
            PreviousNext = true;
        }

        public bool Input { get; set; }

        public bool Numeric { get; set; }

        public bool Info { get; set; }

        public bool PreviousNext { get; set; }

        public bool Refresh { get; set; }

        public bool PageSizes { get; set; }

        #endregion

    }

}
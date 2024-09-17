#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

#endregion

namespace DNA3.Models {

    public class FeaturesView {

        #region Model Attributes

        [Display(Name = "Page")]
        public Page FeaturesPage { get; set; }

        [Display(Name = "Section")]
        public Section FeaturesSection { get; set; }

        [Display(Name = "Controllers")]
        public IList<TypeInfo> Controllers { get; set; }

        [Display(Name = "Tag Helpers")]
        public IList<TypeInfo> TagHelpers { get; set; }

        [Display(Name = "View Components")]
        public IList<TypeInfo> ViewComponents { get; set; }

        [Display(Name = "Assemblies")]
        public IList<Assembly> Assemblies { get; set; }

        #endregion

    }

}

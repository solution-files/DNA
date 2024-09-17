using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNA3.Models {

    public class HomeView {

        public IList<Application> ApplicationList { get; set; }
        public IList<Article> SliderList { get; set; }
        public Section FeatureSection { get; set; }
        public IList<Article> FeatureList { get; set; }
        public Section FooterSection { get; set; }

    }

}

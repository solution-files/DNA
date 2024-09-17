using System;
using System.Collections.Generic;

#nullable disable

namespace DNA3.Models {
    public partial class Role {
        public int RoleId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

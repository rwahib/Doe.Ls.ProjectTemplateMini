using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Doe.Ls.EntityBase.Models {
    public class SelectListItemExtension : SelectListItem {
        public override string ToString()
        {
            return $"{this.Value}-{this.Text} {(this.Selected ? "Selected" : string.Empty)}";
        }
    }
}

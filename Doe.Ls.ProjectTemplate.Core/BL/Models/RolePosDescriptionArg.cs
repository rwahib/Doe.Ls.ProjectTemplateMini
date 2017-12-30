using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models {
    public class RolePosDescriptionArg:EventArgs {
        public RolePosDescriptionArg(RolePositionDescription rolePositionDescription, string message)
        {
            RolePositionDescription = rolePositionDescription;
            Message = message;
        }

        public RolePositionDescription RolePositionDescription { get; private set; }
        public string Message { get; private set; }
        public override string ToString()
        {
            return
                $"{RolePositionDescription.RolePositionDescId}-{RolePositionDescription.DocNumber}-{RolePositionDescription.Title}-{RolePositionDescription.StatusValue}";
        }
    }
    }

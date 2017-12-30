using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
    {
    public class CloneActionModel
        {
        public int SourceRolePositionDescId { get; set; }
        public int SourcePositionId { get; set; }
        public string NewPositionNumber { get; set; }
        public string NewDocNumber { get; set; }
        public string NewGradeCode { get; set; }

        public string NewPositionTitle { get; set; }

        public Position SourcePosition { get; set; }

        public PositionDescription SourcePositionDesc { get; set; }
        public RoleDescription SourceRoleDesc { get; set; }

    }

    }

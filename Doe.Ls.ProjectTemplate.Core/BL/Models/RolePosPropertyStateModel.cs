using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
    {
    public class RolePosPropertyStateModel
        {
        public  bool TitleEnabled { get; set; }
        public  bool GradeEnabled { get; set; }
        public  bool DocumentNumberEnabled { get; set; }

        public static RolePosPropertyStateModel AllEnabled => new RolePosPropertyStateModel
        {
            DocumentNumberEnabled = true,
            GradeEnabled = true,
            TitleEnabled = true
            };
        public static RolePosPropertyStateModel AllDisabled => new RolePosPropertyStateModel
            {
            DocumentNumberEnabled = false,
            GradeEnabled = false,
            TitleEnabled = false
            };
        }
    }

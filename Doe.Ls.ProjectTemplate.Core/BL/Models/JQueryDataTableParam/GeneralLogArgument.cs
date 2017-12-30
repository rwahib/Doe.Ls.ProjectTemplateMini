using Doe.Ls.EntityBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam
    {
    public class GeneralLogArgument : JQueryDataTableParamModel
        {
        public string LogAction { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        }
    }

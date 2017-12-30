using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doe.Ls.EntityBase.Models;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam
    {
    public class JQueryDatatableParamPositionExtension : BasicStructureArgument
        {
        public string PositionNumber { get; set; }
        
        public int FunctionalAreaId { get; set; }
        
        public string[] PosStatusCode { get; set; }
        
        public bool HasSession { get; set; }        
        }
    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
{
   public class PositionChartFilterParams
    {
        public string DivisionCode { get; set; }
        public int DirectorateId { get; set; }
        public int BusinessUnitId { get; set; }
        public int UnitId { get; set; }
        //public int NoOfLevels { get; set; }
    }
}

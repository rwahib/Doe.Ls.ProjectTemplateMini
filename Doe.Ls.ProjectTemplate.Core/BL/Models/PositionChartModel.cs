using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.PositionDescriptions.Core.BL.Models
{
    public class PositionChartModel
    {
        public int id { get; set; }
        public int? parent { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string templateName { get; set; }
        public string groupTitle { get; set; }
        public string groupTitleColor { get; set; }
        public string itemTitleColor { get; set; }
        public string label { get; set; }
        public string labelSize { get; set; }
        public int positionId { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string positionNumber { get; set; }

        public int itemType { get; set; }
        public bool canPrint { get; set; }
        public int unitId { get; set; }
        public string unitName { get; set; }
        public string positionTitle { get; set; }
        public int? DescId { get; set; }
        public string DescUrl { get; set; }
        public string location { get; set; }

        //public string adviserPlacementType { get; set; }
    }
}

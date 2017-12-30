using System.Collections.Generic;

namespace Doe.Ls.EntityBase.Models
{
    public class DataTableResult : ResultBase
    {
        public string sEcho { get; set; }
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public IEnumerable<object> aaData { get; set; }
    }
}

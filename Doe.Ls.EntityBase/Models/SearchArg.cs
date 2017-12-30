using System.Collections.Generic;

namespace Doe.Ls.EntityBase.Models {
    public class SearchArg {

        public string Search { get; set; }
        public string Status { get; set; }

        public Dictionary<string, string> Tags { get; set; }

        public int Start { get; set; }
        
        public int Length { get; set; }
        
        public string OrderByColumnName { get; set; }
        
        public bool Ascending { get; set; }

        public SearchArg()
        {
            Tags=new Dictionary<string, string>();
        }

        public static SearchArg To(JQueryDataTableParamModel param)
        {
            return new SearchArg
            {
               
                Search = param.sSearch,
                Status = param.sSearch2,
                Ascending = param.sSortDir_0 == "asc",
                Length = param.iDisplayLength,
                Start = param.iDisplayStart,
                OrderByColumnName = param.sColumns.Split(',')[param.iSortCol_0]
                
            };

        }
    }
}

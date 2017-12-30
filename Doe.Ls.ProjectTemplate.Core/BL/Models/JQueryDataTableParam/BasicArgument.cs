using Doe.Ls.EntityBase.Models;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam
    {
    public class BasicArgument : JQueryDataTableParamModel
        {
        public int StatusId { get; set; }

        public string[] StatusCode { get; set; }

        public string[] GradeCode { get; set; }
        public string GradeType { get; set; }

        }
   

    }
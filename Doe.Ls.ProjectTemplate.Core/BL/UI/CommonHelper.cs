using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI
{
    public static class CommonHelper
    {
        public static string ConstructDocNumber(HttpRequestBase request)
        {

            var docNumberPart1 = request["DocNumberPart1"];
            var docNumberPart2 = request["DocNumberPart2"];
            if (string.IsNullOrEmpty(docNumberPart1) || string.IsNullOrEmpty(docNumberPart2))
            {
                throw new InvalidOperationException("Doc number is required");
            }
            return "DOC" + docNumberPart1 + "/" + docNumberPart2;
        }

        public static string GetNameOfPSSEType()
        {
            return Enum.GetName(typeof(Enums.GradeType), 2);
        }

        public static string GetPSSE1Code()
        {
            return Enum.GetName(typeof(Enums.PSSEType), 1);
        }

        public static string GetPSSE1DGCode()
        {
            return Enum.GetName(typeof(Enums.PSSEType), 2);
        }

        public static string GetPSSE2Code()
        {
            return Enum.GetName(typeof(Enums.PSSEType), 3);
        }

        public static string GetPSSE3Code()
        {
            return Enum.GetName(typeof(Enums.PSSEType), 4);
        }

        public static int GetUIListCount(string txt)
        {
             var count = Regex.Matches(Regex.Escape(txt), "<li>").Count;
            return count;
        }

        public static Result ValidBulletPoints(string stringList, int minimum, int maximum, string propertyName)
        {
            var result = new Result();
            result.Status = Status.Success;
            result.Message = "OK";

            int count = Regex.Matches(stringList, "<li>").Cast<Match>().Count();

            if (!string.IsNullOrEmpty(stringList))
            {
                if (count == 0)
                {
                    result.Status = Status.Error;
                    result.Message = propertyName + ", please apply bullet points.";
                }
                else if (count < minimum || count > maximum)
                {
                    result.Status = Status.Error;
                    //result.Message = propertyName + ", the bullet points are between " + minimum + " and " + maximum +
                    //                 ".";
                    result.Message = propertyName + " must consist of " + minimum + " - " + maximum +
                                    " bullet points.";
                  }
            }

            return result;
        }
    }
}
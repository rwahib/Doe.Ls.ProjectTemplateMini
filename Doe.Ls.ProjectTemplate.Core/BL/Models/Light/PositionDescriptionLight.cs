


using System;
using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class PositionDescriptionLight {
        
        public System.Int32 PositionDescriptionId {get;set;}
        
        public System.String BriefRoleStatement {get;set;}
        
        public System.String StatementOfDuties {get;set;}
         public string GradeCode { get; set; }
        public string GradeTitle { get; set; }
        public string DocNumber { get; set; }
         public string Title { get; set; }
        public string DateOfApproval { get; set; }
        public string StatusValue { get; set; }

        public string LastModifiedDate { get; set; }

        public int LinkedPositions { get; set; }
        public Enums.Privilege Privilege { get; set; }

        public override string ToString()
            {
            var    st = $"{PositionDescriptionId}-{DocNumber}-{Title}";
                st += $"-{StatusValue}";
                
            return st;
            }
        }
}

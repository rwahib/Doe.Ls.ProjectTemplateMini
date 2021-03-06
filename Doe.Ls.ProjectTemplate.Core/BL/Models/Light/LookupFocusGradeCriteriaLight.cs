


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class LookupFocusGradeCriteriaLight {
        
        public System.Int32 LookupId {get;set;}
        
        public System.Int32 FocusId {get;set;}
        
        public System.String GradeCode {get;set;}
        
        public System.Int32 SelectionCriteriaId {get;set;}
        
        public System.DateTime LastUpdatedDate {get;set;}
        
        public System.String LastUpdatedBy {get;set;}
         public string FocusName { get; set; }
         public string Criteria { get; set; }
         public string GradeName { get; set; }
         public bool IsMandatory { get; set; }
     }
}




using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class SelectionCriteriaLight {
        
        public System.Int32 SelectionCriteriaId {get;set;}
        
        public System.String Criteria {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String LastModifiedBy {get;set;}
  
    }
}

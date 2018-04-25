 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class ExecutiveLight {
        
        public System.String ExecutiveCod {get;set;}
        
        public System.String ExecutiveTitle {get;set;}
        
        public System.String ExecutiveDescription {get;set;}
        
        public System.String CustomClass {get;set;}
        
        public System.Int32 StatusId {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String LastModifiedBy {get;set;}
        
        public System.String DefaultExecutiveOverview {get;set;}
  
    }
}




using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class RolePositionDescriptionLight {
        
        public System.Int32 RolePositionDescId {get;set;}
        
        public System.Int32 StatusId {get;set;}
        
        public System.Int32 Version {get;set;}
        
        public System.String Title {get;set;}
        
        public System.String DocNumber {get;set;}
        
        public System.String GradeCode {get;set;}
        
        public System.DateTime DateOfApproval {get;set;}
        
        public System.Boolean IsPositionDescription {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.String LastModifiedBy {get;set;}
  
    }
}

 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class SysRoleLight {
        
        public System.Int32 RoleId {get;set;}
        
        public System.String RoleTitle {get;set;}
        
        public System.String RoleApiName {get;set;}
        
        public System.String RoleDescription {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String LastModifiedBy {get;set;}
  
    }
}

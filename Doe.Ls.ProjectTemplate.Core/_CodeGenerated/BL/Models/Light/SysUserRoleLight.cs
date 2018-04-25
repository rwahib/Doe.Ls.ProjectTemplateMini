 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class SysUserRoleLight {
        
        public System.String UserId {get;set;}
        
        public System.Int32 RoleId {get;set;}
        
        public System.String StructureId {get;set;}
        
        public System.Int32 OrgLevelId {get;set;}
        
        public System.DateTime ActiveFrom {get;set;}
        
        public System.DateTime ActiveTo {get;set;}
        
        public System.String Note {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String LastModifiedBy {get;set;}
  
    }
}

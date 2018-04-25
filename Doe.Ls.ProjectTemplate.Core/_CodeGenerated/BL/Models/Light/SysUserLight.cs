 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class SysUserLight {
        
        public System.String UserId {get;set;}
        
        public System.String FirstName {get;set;}
        
        public System.String LastName {get;set;}
        
        public System.String Email {get;set;}
        
        public System.String PrimaryPhone {get;set;}
        
        public System.String Note {get;set;}
        
        public System.Boolean Active {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String LastModifiedBy {get;set;}
  
    }
}

 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class SysMessageLight {
        
        public System.String Code {get;set;}
        
        public System.String MessageFormat {get;set;}
        
        public System.Int32 MsgCategoryId {get;set;}
        
        public System.String MessageHint {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.String LastModifiedBy {get;set;}
  
    }
}

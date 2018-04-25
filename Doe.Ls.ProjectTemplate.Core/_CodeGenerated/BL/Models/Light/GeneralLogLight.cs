 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class GeneralLogLight {
        
        public System.Int32 LogId {get;set;}
        
        public System.String Action {get;set;}
        
        public System.String Context {get;set;}
        
        public System.DateTime CreationDate {get;set;}
        
        public System.String Username {get;set;}
        
        public System.Int32 RoleId {get;set;}
        
        public System.String Note {get;set;}
  
    }
}

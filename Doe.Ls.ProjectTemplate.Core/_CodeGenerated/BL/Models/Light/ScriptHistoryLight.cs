 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class ScriptHistoryLight {
        
        public System.String ScriptNumber {get;set;}
        
        public System.String ScriptName {get;set;}
        
        public System.DateTime RunDate {get;set;}
        
        public System.String RunBy {get;set;}
        
        public System.String Comments {get;set;}
  
    }
}

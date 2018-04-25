 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class GlobalSettingLight {
        
        public System.Int32 SettingsKey {get;set;}
        
        public System.String PropertyCode {get;set;}
        
        public System.Boolean PropertyBooleanValue {get;set;}
        
        public System.String PropertyValue {get;set;}
        
        public System.String EntityContextCode {get;set;}
        
        public System.String EntityContextValue {get;set;}
        
        public System.String Tag {get;set;}
  
    }
}

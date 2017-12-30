


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class CapabilityBehaviourIndicatorLight {
        
        public System.Int32 CapabilityNameId {get;set;}
        
        public System.Int32 CapabilityLevelId {get;set;}
        
        public System.String IndicatorContext {get;set;}
        
        public System.DateTime DateCreated {get;set;}
        
        public System.DateTime LastUpdated {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.String LastModifiedBy {get;set;}
         public string CapabilityLevelName { get; set; }
         public string CapabilityName { get; set; }
     }
}

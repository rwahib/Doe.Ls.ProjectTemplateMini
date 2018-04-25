 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class CapabilityNameLight {
        
        public System.Int32 CapabilityNameId {get;set;}
        
        public System.String Name {get;set;}
        
        public System.String CapabilityDescription {get;set;}
        
        public System.Int32 CapabilityGroupId {get;set;}
        
        public System.DateTime DateCreated {get;set;}
        
        public System.DateTime LastUpdated {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.String LastModifiedBy {get;set;}
  
    }
}

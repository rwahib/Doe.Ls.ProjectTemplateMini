


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class CapabilityGroupLight {
        
        public System.Int32 CapabilityGroupId {get;set;}
        
        public System.String GroupName {get;set;}
        
        public System.String GroupDescription {get;set;}
        
        public System.Int32 DisplayOrder {get;set;}
        
        public System.DateTime DateCreated {get;set;}
        
        public System.DateTime LastUpdated {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.String LastModifiedBy {get;set;}
        
        public System.String GroupImage {get;set;}
  
    }
}

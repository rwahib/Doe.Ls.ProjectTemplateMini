


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class CapabilityLevelLight {
        
        public System.Int32 CapabilityLevelId {get;set;}
        
        public System.String LevelName {get;set;}
        
        public System.Int32 DisplayOrder {get;set;}
        
        public System.Int32 LevelOrder {get;set;}
        
        public System.DateTime DateCreated {get;set;}
        
        public System.DateTime LastUpdated {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.String LastModifiedBy {get;set;}
  
    }
}

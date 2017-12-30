


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class DirectorateLight {
        
        public System.Int32 DirectorateId {get;set;}
        
        public System.String ExecutiveCod {get;set;}
        
        public System.String DirectorateName {get;set;}
        
        public System.String DirectorateDescription {get;set;}
        
        public System.String DirectorateOverview {get;set;}
        
        public System.String DirectorateCustomClass {get;set;}
        
        public System.Int32 StatusId {get;set;}
        
        public System.Int32 DirectorateOrder {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String LastModifiedBy {get;set;}
  
    }
}

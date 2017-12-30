


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class UnitLight {
        
        public System.Int32 UnitId {get;set;}
        
        public System.String UnitName {get;set;}
        
        public System.String UnitDescription {get;set;}
        
        public System.Int32 BUnitId {get;set;}
        
        public System.Int32 FunctionalAreaId {get;set;}
        
        public System.Int32 ReportToUnit {get;set;}
        
        public System.Int32 HierarchyId {get;set;}
        
        public System.Int32 TeamTypeId {get;set;}
        
        public System.String UnitCustomClass {get;set;}
        
        public System.Int32 StatusId {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String LastModifiedBy {get;set;}
  
    }
}

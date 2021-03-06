 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class EmployeePositionLight {
        
        public System.Int32 EmployeeId {get;set;}
        
        public System.Int32 PositionId {get;set;}
        
        public System.Int32 StatusId {get;set;}
        
        public System.Boolean DisplayInOrgChart {get;set;}
        
        public System.String Reason {get;set;}
        
        public System.DateTime FromDate {get;set;}
        
        public System.DateTime ToDate {get;set;}
        
        public System.String LastModifiedBy {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String CreatedBy {get;set;}
  
    }
}

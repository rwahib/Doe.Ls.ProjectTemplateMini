


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class EmployeeLight {
        
        public System.Int32 EmployeeId {get;set;}
        
        public System.String FirstName {get;set;}
        
        public System.String LastName {get;set;}
        
        public System.Decimal CostCentreNumber {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.String LastModifiedBy {get;set;}
        
        public System.Int32 StatusId {get;set;}
        public Enums.Privilege Privilege { get; set; }
        }
}

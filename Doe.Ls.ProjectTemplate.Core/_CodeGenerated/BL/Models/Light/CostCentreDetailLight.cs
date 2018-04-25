 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class CostCentreDetailLight {
        
        public System.Int32 PositionId {get;set;}
        
        public System.String CostCentre {get;set;}
        
        public System.String Fund {get;set;}
        
        public System.String PayrollWBS {get;set;}
        
        public System.String RCCJDEPayrollCode {get;set;}
        
        public System.String GLAccount {get;set;}
        
        public System.DateTime LastUpdatedDate {get;set;}
        
        public System.String LastUpdatedBy {get;set;}
  
    }
}

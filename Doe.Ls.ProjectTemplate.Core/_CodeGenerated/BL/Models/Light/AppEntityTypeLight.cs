 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class AppEntityTypeLight {
        
        public System.Int32 EntityTypeId {get;set;}
        
        public System.String EntityApiName {get;set;}
        
        public System.String EntityTitle {get;set;}
        
        public System.String EntityDescription {get;set;}
        
        public System.Boolean SysAdminDashboard {get;set;}
        
        public System.Boolean PowerUserDashboard {get;set;}
        
        public System.Boolean HighPriority {get;set;}
  
    }
}

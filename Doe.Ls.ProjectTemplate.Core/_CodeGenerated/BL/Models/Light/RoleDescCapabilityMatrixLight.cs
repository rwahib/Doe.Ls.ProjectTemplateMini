 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class RoleDescCapabilityMatrixLight {
        
        public System.String GradeCode {get;set;}
        
        public System.Int32 Foundational_Min {get;set;}
        
        public System.Int32 Foundational_Max {get;set;}
        
        public System.Int32 Intermediate_Min {get;set;}
        
        public System.Int32 Intermediate_Max {get;set;}
        
        public System.Int32 Adept_Min {get;set;}
        
        public System.Int32 Adept_Max {get;set;}
        
        public System.Int32 Advanced_Min {get;set;}
        
        public System.Int32 Advanced_Max {get;set;}
        
        public System.Int32 HighlyAdvanced_Min {get;set;}
        
        public System.Int32 HighlyAdvanced_Max {get;set;}
        
        public System.Int32 FocusCapabilities_Min {get;set;}
        
        public System.Int32 FocusCapabilities_Max {get;set;}
        
        public System.String Notes {get;set;}
  
    }
}

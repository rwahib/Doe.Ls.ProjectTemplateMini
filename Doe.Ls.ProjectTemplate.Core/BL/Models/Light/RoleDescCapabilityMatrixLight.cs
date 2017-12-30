


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class RoleDescCapabilityMatrixLight {
        
        public System.String GradeCode {get;set;}
        public System.String Foundational_Range {get;set;}
        public System.String Intermediate_Range { get;set;}
        public System.String Adept_Range { get;set;}
        public System.String Advanced_Range { get;set;}
        public System.String HighlyAdvanced_Range { get;set;}
        public System.String FocusCapabilities_Range { get;set;}
        public System.String Notes {get;set;}
    }
}

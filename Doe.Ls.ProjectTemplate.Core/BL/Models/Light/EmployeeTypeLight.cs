


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class EmployeeTypeLight {
        
        public System.String EmployeeTypeCode {get;set;}
        
        public System.String EmployeeTypeName {get;set;}
        
        public System.String EmployeeTypeDescription {get;set;}

        public Enums.Privilege Privilege { get; set; }

        }
}

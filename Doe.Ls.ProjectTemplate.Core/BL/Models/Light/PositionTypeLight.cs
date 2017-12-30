


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class PositionTypeLight {
        
        public System.String PositionTypeCode {get;set;}
        
        public System.String PositionTypeName {get;set;}
        
        public System.String PositionTypeDescription {get;set;}

        public Enums.Privilege Privilege { get; set; }

        }
}

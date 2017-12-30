


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class OccupationTypeLight {
        
        public System.String OccupationTypeCode {get;set;}
        
        public System.String OccupationTypeName {get;set;}

        public Enums.Privilege Privilege { get; set; }

        }
}

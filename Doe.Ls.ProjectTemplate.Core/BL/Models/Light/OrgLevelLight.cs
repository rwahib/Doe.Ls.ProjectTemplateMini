


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class OrgLevelLight {
        
        public System.Int32 OrgLevelId {get;set;}
        
        public System.String OrgLevelTitle {get;set;}
        
        public System.String OrgLevelName {get;set;}
        
        public System.String Description {get;set;}

        public Enums.Privilege Privilege { get; set; }

        }
}

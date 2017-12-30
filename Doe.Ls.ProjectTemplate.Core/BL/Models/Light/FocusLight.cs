


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class FocusLight {
        
        public System.Int32 FocusId {get;set;}
        
        public System.String FocusName {get;set;}
        
        public System.Int32 OrderList {get;set;}
        public Enums.Privilege Privilege { get; set; }
        }
}

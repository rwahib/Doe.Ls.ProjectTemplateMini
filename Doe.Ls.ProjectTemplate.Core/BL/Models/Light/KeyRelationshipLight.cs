


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class KeyRelationshipLight {
        
        public System.Int32 KeyRelationshipId {get;set;}
        
        public System.Int32 RoleDescriptionId {get;set;}
        
        public System.Int32 ScopeId {get;set;}
        
        public System.Int32 OrderNumber {get;set;}
        
        public System.String Who {get;set;}
        
        public System.String Why {get;set;}
        
        public System.DateTime DateCreated {get;set;}
        
        public System.String ModifiedUserName {get;set;}
        
        public System.DateTime LastUpdated {get;set;}

        public string ScopeName { get; set; }

     }
}

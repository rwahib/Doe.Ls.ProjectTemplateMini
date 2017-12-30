


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class FunctionalAreaLight {
        
        public System.Int32 FuncationalAreaId {get;set;}
        
        public System.String AreanName {get;set;}
        
        public System.String AreaDescription {get;set;}
        
        public System.Int32 DirectorateId {get;set;}
        
        public System.String AreaCustomClass {get;set;}
        public System.String DirectorateName { get; set; }
        
        public System.String StatusName { get; set; }

        public System.String ExecutiveTitle { get; set; }
        
        public System.Int32 StatusId {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String LastModifiedBy {get;set;}

        public Enums.Privilege Privilege { get; set; }

        }
}

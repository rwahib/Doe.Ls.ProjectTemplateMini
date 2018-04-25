 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class AppObjectInfoLight {
        
        public System.String ObjectName {get;set;}
        
        public System.Int32 CounterValue {get;set;}
        
        public System.Double AggregatedValueA {get;set;}
        
        public System.Double AggregatedValueB {get;set;}
        
        public System.String Metadata {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String lastModifiedUser {get;set;}
  
    }
}

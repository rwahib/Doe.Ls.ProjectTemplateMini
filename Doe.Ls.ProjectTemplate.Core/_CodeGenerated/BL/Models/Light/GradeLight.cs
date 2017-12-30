


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class GradeLight {
        
        public System.String GradeCode {get;set;}
        
        public System.String GradeTitle {get;set;}
        
        public System.String Award {get;set;}
        
        public System.Decimal AwardMaxRates {get;set;}
        
        public System.Boolean TeachingBased {get;set;}
        
        public System.String GradeType {get;set;}
        
        public System.Int32 StatusId {get;set;}
        
        public System.String Message {get;set;}
  
    }
}

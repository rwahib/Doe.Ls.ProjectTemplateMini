 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class PositionHistoryLight {
        
        public System.Int32 PositionHistoryId {get;set;}
        
        public System.Int32 PositionId {get;set;}
        
        public System.String Action {get;set;}
        
        public System.String StatusFrom {get;set;}
        
        public System.String StatusTo {get;set;}
        
        public System.String AdditionalInfo {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.String CreatedBy {get;set;}
  
    }
}

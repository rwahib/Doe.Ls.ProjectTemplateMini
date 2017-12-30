


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class PositionNoteLight {
        
        public System.Int32 PositionNoteId {get;set;}
        
        public System.Int32 PositionId {get;set;}
        
        public System.String Notes {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
  
    }
}




using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class WfTaskLight {
        
        public System.Int32 WfTaskId {get;set;}
        
        public System.Int32 WfObjectTypeId {get;set;}
        
        public System.Int32 WfObjectId {get;set;}
        
        public System.String WfObjecTitle {get;set;}
        
        public System.Int32 WfObjecCurrentStatusId {get;set;}
        
        public System.Int32 WfObjecPrevStatusId {get;set;}
        
        public System.Int32 WfActionId {get;set;}
        
        public System.String WfStepInfo {get;set;}
        
        public System.String WfComment {get;set;}
        
        public System.String WfRecommendation {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.Int32 CreatedBySysRoleId {get;set;}
        
        public System.Int32 ActionWaitingForSysRoleId {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
  
    }
}

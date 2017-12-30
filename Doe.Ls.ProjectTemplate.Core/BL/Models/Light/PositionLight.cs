


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class PositionLight {
        
        public System.Int32 PositionId {get;set;}
        
        public System.Int32 ReportToPositionId {get;set;}
        
        public System.String PositionNumber {get;set;}
        
        public System.Int32 RolePositionDescriptionId {get;set;}
        public System.String DOCNumber { get; set; }

        public System.Int32 UnitId {get;set;}
        
        public System.String PositionTitle {get;set;}
        
        public System.String Description {get;set;}
        
        public System.Int32 PositionLevelId {get;set;}
        
        public System.Int32 StatusId {get;set;}
        public System.String StatusName { get; set; }

        public System.String PositionPath {get;set;}
        
        public System.Int32 LocationId {get;set;}
        
        public System.DateTime CreatedDate {get;set;}
        
        public System.DateTime LastModifiedDate {get;set;}
        
        public System.String CreatedBy {get;set;}
        
        public System.String LastModifiedBy {get;set;}
         public string UnitName { get; set; }
         public string ParentPositionTitle { get; set; }
         public string PositionLevel { get; set; }
        public string Grade { get; set; }
         public bool CanViewRolePosDesc { get; set; }
         public string EmployeeType { get; set; }
         public string OccupationType { get; set; }
         public string PositionType { get; set; }
        public Enums.Privilege Privilege { get; set; }
        public override string ToString()
            {
            return $"{PositionId}-{PositionNumber}-{this.PositionTitle}-{UnitName}-{StatusName}";
            }
        }
}




using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light {
     public class RoleDescriptionLight {
        
        public System.Int32 RoleDescriptionId {get;set;}
        
        public System.String Cluster {get;set;}
        
        public System.String SeniorExecutiveWorkLevelStandards {get;set;}
        
        public System.String ANZSCOCode {get;set;}
        
        public System.String PCATCode {get;set;}
        
        public string DateOfApproval {get;set;}
        
        public System.String AgencyOverview {get;set;}
        
        public System.String Agency {get;set;}
        
        public System.String AgencyWebsite {get;set;}
        
        public System.String RolePrimaryPurpose {get;set;}
        
        public System.String KeyAccountabilities {get;set;}
        
        public System.String KeyChallenges {get;set;}
        
        public System.String DecisionMaking {get;set;}
        
        public System.String ReportingLine {get;set;}
        
        public System.String DirectReports {get;set;}
        
        public System.String BudgetExpenditure {get;set;}
        
        public System.String BudgetExpenditureValue {get;set;}
        
        public System.String EssentialRequirements {get;set;}
        
        public System.String RoleCapabilities {get;set;}
        
        public System.String CapabilitySummary {get;set;}
        
        public System.String FocusCapabilities {get;set;}
        
        public System.String LastModifiedBy {get;set;}
        
        public System.DateTime CreatedDate {get;set;}

        public string LastModifiedDate { get; set; }
        
        public System.String CreatedBy {get;set;}
        
        public System.String VersionStatus {get;set;}
        
        public System.String OldPDFileName {get;set;}
        
        public System.Boolean ManagerRole {get;set;}
         public string GradeTitle { get; set; }
         public string DocNumber { get; set; }
         public string Title { get; set; }
         public string Status { get; set; }
         public int LinkedPositions { get; set; }

        public Enums.Privilege Privilege { get; set; }

        public override string ToString()
            {
            var st = $"{RoleDescriptionId}-{DocNumber}-{Title}";
            st += $"-{Status}";

            return st;
            }
        }
}




using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(RoleDescriptionMetadata))]
    public partial class RoleDescription
    {
        public override string ToString()
        {
            return string.Format("{0}", this.OldPDFileName);
        }
    }

    public class RoleDescriptionMetadata
    {
        
        [Display(Name ="Role description id" )]
        [Required(ErrorMessage = "Role description id is required")]
        public System.Int32 RoleDescriptionId {get;set;}
        
        [Display(Name ="Cluster" )]
        [MaxLength(250, ErrorMessage = "Exceeding the max length, allowed only 250 character")]
        public System.String Cluster {get;set;}
        
        [Display(Name ="Senior executive work level standards" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1024, ErrorMessage = "Exceeding the max length, allowed only 1024 character")]
        public System.String SeniorExecutiveWorkLevelStandards {get;set;}
        
        [Display(Name ="A n z s c o code" )]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String ANZSCOCode {get;set;}
        
        [Display(Name ="P c a t code" )]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String PCATCode {get;set;}
        
        [Display(Name ="Agency overview" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String AgencyOverview {get;set;}
        
        [Display(Name ="Agency" )]
        [MaxLength(250, ErrorMessage = "Exceeding the max length, allowed only 250 character")]
        public System.String Agency {get;set;}
        
        [Display(Name ="Agency website" )]
        [MaxLength(250, ErrorMessage = "Exceeding the max length, allowed only 250 character")]
        public System.String AgencyWebsite {get;set;}
        
        [Display(Name ="Role primary purpose" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String RolePrimaryPurpose {get;set;}
        
        [Display(Name ="Key accountabilities" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String KeyAccountabilities {get;set;}
        
        [Display(Name ="Key challenges" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String KeyChallenges {get;set;}
        
        [Display(Name ="Decision making" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String DecisionMaking {get;set;}
        
        [Display(Name ="Reporting line" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String ReportingLine {get;set;}
        
        [Display(Name ="Direct reports" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String DirectReports {get;set;}
        
        [Display(Name ="Budget expenditure" )]
        [MaxLength(255, ErrorMessage = "Exceeding the max length, allowed only 255 character")]
        public System.String BudgetExpenditure {get;set;}
        
        [Display(Name ="Budget expenditure value" )]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String BudgetExpenditureValue {get;set;}
        
        [Display(Name ="Budget extra notes" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String BudgetExtraNotes {get;set;}
        
        [Display(Name ="Essential requirements" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String EssentialRequirements {get;set;}
        
        [Display(Name ="Role capability items" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String RoleCapabilityItems {get;set;}
        
        [Display(Name ="Capability summary" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String CapabilitySummary {get;set;}
        
        [Display(Name ="Focus capabilities" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String FocusCapabilities {get;set;}
        
        [Display(Name ="Last modified by" )]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String LastModifiedBy {get;set;}
        
        [Display(Name ="Created date" )]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime CreatedDate {get;set;}
        
        [Display(Name ="Last modified date" )]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}")]
        [DataType(DataType.DateTime)]
        public System.DateTime LastModifiedDate {get;set;}
        
        [Display(Name ="Created by" )]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String CreatedBy {get;set;}
        
        [Display(Name ="Version status" )]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String VersionStatus {get;set;}
        
        [Display(Name ="Old p d file name" )]
        [MaxLength(150, ErrorMessage = "Exceeding the max length, allowed only 150 character")]
        public System.String OldPDFileName {get;set;}
        
        [Display(Name ="Manager role" )]
        public System.Boolean ManagerRole {get;set;}
  



        
        [Display(Name ="Key relationships" )]
        public object KeyRelationships {get;set;}
        
        [Display(Name ="Role capabilities" )]
        public object RoleCapabilities {get;set;}
        
        [Display(Name ="Role position description" )]
        public object RolePositionDescription {get;set;}
  

    }
}


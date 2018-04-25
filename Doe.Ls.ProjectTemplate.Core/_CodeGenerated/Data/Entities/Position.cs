 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(PositionMetadata))]
    public partial class Position
    {
        public override string ToString()
        {
            return string.Format("{0}", this.PositionTitle);
        }
    }

    public class PositionMetadata
    {
        
        [Display(Name ="Position id" )]
        public System.Int32 PositionId {get;set;}
        
        [Display(Name ="Report to position id" )]
        [Required(ErrorMessage = "Report to position id is required")]
        public System.Int32 ReportToPositionId {get;set;}
        
        [Display(Name ="Position number" )]
        [Required(ErrorMessage = "Position number is required")]
        [MaxLength(10, ErrorMessage = "Exceeding the max length, allowed only 10 character")]
        public System.String PositionNumber {get;set;}
        
        [Display(Name ="Role position description id" )]
        [Required(ErrorMessage = "Role position description id is required")]
        public System.Int32 RolePositionDescriptionId {get;set;}
        
        [Display(Name ="Unit id" )]
        [Required(ErrorMessage = "Unit id is required")]
        public System.Int32 UnitId {get;set;}
        
        [Display(Name ="Position title" )]
        [MaxLength(250, ErrorMessage = "Exceeding the max length, allowed only 250 character")]
        public System.String PositionTitle {get;set;}
        
        [Display(Name ="Description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String Description {get;set;}
        
        [Display(Name ="Position level id" )]
        [Required(ErrorMessage = "Position level id is required")]
        public System.Int32 PositionLevelId {get;set;}
        
        [Display(Name ="Status id" )]
        [Required(ErrorMessage = "Status id is required")]
        public System.Int32 StatusId {get;set;}
        
        [Display(Name ="Position path" )]
        [Required(ErrorMessage = "Position path is required")]
        [MaxLength(500, ErrorMessage = "Exceeding the max length, allowed only 500 character")]
        public System.String PositionPath {get;set;}
        
        [Display(Name ="Location id" )]
        [Required(ErrorMessage = "Location id is required")]
        public System.Int32 LocationId {get;set;}
        
        [Display(Name ="Created date" )]
        [Required(ErrorMessage = "Created date is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime CreatedDate {get;set;}
        
        [Display(Name ="Last modified date" )]
        [Required(ErrorMessage = "Last modified date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}")]
        [DataType(DataType.DateTime)]
        public System.DateTime LastModifiedDate {get;set;}
        
        [Display(Name ="Created by" )]
        [Required(ErrorMessage = "Created by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String CreatedBy {get;set;}
        
        [Display(Name ="Last modified by" )]
        [Required(ErrorMessage = "Last modified by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String LastModifiedBy {get;set;}
        
        [Display(Name ="Division overview" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String DivisionOverview {get;set;}
  



        
        [Display(Name ="Cost centre detail" )]
        public object CostCentreDetail {get;set;}
        
        [Display(Name ="Employee positions" )]
        public object EmployeePositions {get;set;}
        
        [Display(Name ="Location" )]
        public object Location {get;set;}
        
        [Display(Name ="Positions" )]
        public object Positions {get;set;}
        
        [Display(Name ="Parent position" )]
        public object ParentPosition {get;set;}
        
        [Display(Name ="Position level" )]
        public object PositionLevel {get;set;}
        
        [Display(Name ="Role position description" )]
        public object RolePositionDescription {get;set;}
        
        [Display(Name ="Status value" )]
        public object StatusValue {get;set;}
        
        [Display(Name ="Unit" )]
        public object Unit {get;set;}
        
        [Display(Name ="Position histories" )]
        public object PositionHistories {get;set;}
        
        [Display(Name ="Position information" )]
        public object PositionInformation {get;set;}
  

    }
}


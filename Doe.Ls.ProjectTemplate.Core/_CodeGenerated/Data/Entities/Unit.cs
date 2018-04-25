 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(UnitMetadata))]
    public partial class Unit
    {
        public override string ToString()
        {
            return string.Format("{0}", this.UnitName);
        }
    }

    public class UnitMetadata
    {
        
        [Display(Name ="Unit id" )]
        [Required(ErrorMessage = "Unit id is required")]
        public System.Int32 UnitId {get;set;}
        
        [Display(Name ="Unit name" )]
        [Required(ErrorMessage = "Unit name is required")]
        [MaxLength(150, ErrorMessage = "Exceeding the max length, allowed only 150 character")]
        public System.String UnitName {get;set;}
        
        [Display(Name ="Unit description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String UnitDescription {get;set;}
        
        [Display(Name ="B unit id" )]
        [Required(ErrorMessage = "B unit id is required")]
        public System.Int32 BUnitId {get;set;}
        
        [Display(Name ="Functional area id" )]
        [Required(ErrorMessage = "Functional area id is required")]
        public System.Int32 FunctionalAreaId {get;set;}
        
        [Display(Name ="Report to unit" )]
        [Required(ErrorMessage = "Report to unit is required")]
        public System.Int32 ReportToUnit {get;set;}
        
        [Display(Name ="Hierarchy id" )]
        [Required(ErrorMessage = "Hierarchy id is required")]
        public System.Int32 HierarchyId {get;set;}
        
        [Display(Name ="Team type id" )]
        [Required(ErrorMessage = "Team type id is required")]
        public System.Int32 TeamTypeId {get;set;}
        
        [Display(Name ="Unit custom class" )]
        [MaxLength(123, ErrorMessage = "Exceeding the max length, allowed only 123 character")]
        public System.String UnitCustomClass {get;set;}
        
        [Display(Name ="Status id" )]
        [Required(ErrorMessage = "Status id is required")]
        public System.Int32 StatusId {get;set;}
        
        [Display(Name ="Created date" )]
        [Required(ErrorMessage = "Created date is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime CreatedDate {get;set;}
        
        [Display(Name ="Created by" )]
        [Required(ErrorMessage = "Created by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String CreatedBy {get;set;}
        
        [Display(Name ="Last modified date" )]
        [Required(ErrorMessage = "Last modified date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}")]
        [DataType(DataType.DateTime)]
        public System.DateTime LastModifiedDate {get;set;}
        
        [Display(Name ="Last modified by" )]
        [Required(ErrorMessage = "Last modified by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String LastModifiedBy {get;set;}
  



        
        [Display(Name ="Business unit" )]
        public object BusinessUnit {get;set;}
        
        [Display(Name ="Functional area" )]
        public object FunctionalArea {get;set;}
        
        [Display(Name ="Hierarchy level" )]
        public object HierarchyLevel {get;set;}
        
        [Display(Name ="Status value" )]
        public object StatusValue {get;set;}
        
        [Display(Name ="Team type" )]
        public object TeamType {get;set;}
        
        [Display(Name ="Unit list" )]
        public object UnitList {get;set;}
        
        [Display(Name ="Parent unit" )]
        public object ParentUnit {get;set;}
        
        [Display(Name ="Positions" )]
        public object Positions {get;set;}
  

    }
}


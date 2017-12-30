


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(DirectorateMetadata))]
    public partial class Directorate
    {
        public override string ToString()
        {
            return string.Format("{0}", this.DirectorateName);
        }
    }

    public class DirectorateMetadata
    {
        
        [Display(Name ="Directorate id" )]
        [Required(ErrorMessage = "Directorate id is required")]
        public System.Int32 DirectorateId {get;set;}
        
        [Display(Name ="Executive cod" )]
        [Required(ErrorMessage = "Executive cod is required")]
        [MaxLength(64, ErrorMessage = "Exceeding the max length, allowed only 64 character")]
        public System.String ExecutiveCod {get;set;}
        
        [Display(Name ="Directorate name" )]
        [Required(ErrorMessage = "Directorate name is required")]
        [DataType(DataType.MultilineText)]
        [MaxLength(512, ErrorMessage = "Exceeding the max length, allowed only 512 character")]
        public System.String DirectorateName {get;set;}
        
        [Display(Name ="Directorate description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String DirectorateDescription {get;set;}
        
        [Display(Name ="Directorate overview" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String DirectorateOverview {get;set;}
        
        [Display(Name ="Directorate custom class" )]
        [MaxLength(123, ErrorMessage = "Exceeding the max length, allowed only 123 character")]
        public System.String DirectorateCustomClass {get;set;}
        
        [Display(Name ="Status id" )]
        [Required(ErrorMessage = "Status id is required")]
        public System.Int32 StatusId {get;set;}
        
        [Display(Name ="Directorate order" )]
        [Required(ErrorMessage = "Directorate order is required")]
        public System.Int32 DirectorateOrder {get;set;}
        
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
  



        
        [Display(Name ="Business units" )]
        public object BusinessUnits {get;set;}
        
        [Display(Name ="Executive" )]
        public object Executive {get;set;}
        
        [Display(Name ="Status value" )]
        public object StatusValue {get;set;}
        
        [Display(Name ="Functional areas" )]
        public object FunctionalAreas {get;set;}
        
        [Display(Name ="Locations" )]
        public object Locations {get;set;}
  

    }
}


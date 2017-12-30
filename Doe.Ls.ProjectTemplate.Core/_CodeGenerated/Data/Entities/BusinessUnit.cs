


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(BusinessUnitMetadata))]
    public partial class BusinessUnit
    {
        public override string ToString()
        {
            return string.Format("{0}", this.BUnitName);
        }
    }

    public class BusinessUnitMetadata
    {
        
        [Display(Name ="B unit id" )]
        [Required(ErrorMessage = "B unit id is required")]
        public System.Int32 BUnitId {get;set;}
        
        [Display(Name ="Directorate id" )]
        [Required(ErrorMessage = "Directorate id is required")]
        public System.Int32 DirectorateId {get;set;}
        
        [Display(Name ="Hierarchy id" )]
        [Required(ErrorMessage = "Hierarchy id is required")]
        public System.Int32 HierarchyId {get;set;}
        
        [Display(Name ="B unit name" )]
        [Required(ErrorMessage = "B unit name is required")]
        [MaxLength(150, ErrorMessage = "Exceeding the max length, allowed only 150 character")]
        public System.String BUnitName {get;set;}
        
        [Display(Name ="B unit description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String BUnitDescription {get;set;}
        
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
  



        
        [Display(Name ="Directorate" )]
        public object Directorate {get;set;}
        
        [Display(Name ="Hierarchy level" )]
        public object HierarchyLevel {get;set;}
        
        [Display(Name ="Status value" )]
        public object StatusValue {get;set;}
        
        [Display(Name ="Units" )]
        public object Units {get;set;}
  

    }
}


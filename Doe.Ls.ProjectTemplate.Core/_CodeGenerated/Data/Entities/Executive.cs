 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(ExecutiveMetadata))]
    public partial class Executive
    {
        public override string ToString()
        {
            return string.Format("{0}", this.ExecutiveTitle);
        }
    }

    public class ExecutiveMetadata
    {
        
        [Display(Name ="Executive cod" )]
        [Required(ErrorMessage = "Executive cod is required")]
        [MaxLength(64, ErrorMessage = "Exceeding the max length, allowed only 64 character")]
        public System.String ExecutiveCod {get;set;}
        
        [Display(Name ="Executive title" )]
        [Required(ErrorMessage = "Executive title is required")]
        [DataType(DataType.MultilineText)]
        [MaxLength(512, ErrorMessage = "Exceeding the max length, allowed only 512 character")]
        public System.String ExecutiveTitle {get;set;}
        
        [Display(Name ="Executive description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1024, ErrorMessage = "Exceeding the max length, allowed only 1024 character")]
        public System.String ExecutiveDescription {get;set;}
        
        [Display(Name ="Custom class" )]
        [MaxLength(123, ErrorMessage = "Exceeding the max length, allowed only 123 character")]
        public System.String CustomClass {get;set;}
        
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
        
        [Display(Name ="Default executive overview" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String DefaultExecutiveOverview {get;set;}
  



        
        [Display(Name ="Directorates" )]
        public object Directorates {get;set;}
        
        [Display(Name ="Status value" )]
        public object StatusValue {get;set;}
  

    }
}


 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(FunctionalAreaMetadata))]
    public partial class FunctionalArea
    {
        public override string ToString()
        {
            return string.Format("{0}", this.AreanName);
        }
    }

    public class FunctionalAreaMetadata
    {
        
        [Display(Name ="Funcational area id" )]
        [Required(ErrorMessage = "Funcational area id is required")]
        public System.Int32 FuncationalAreaId {get;set;}
        
        [Display(Name ="Arean name" )]
        [Required(ErrorMessage = "Arean name is required")]
        [DataType(DataType.MultilineText)]
        [MaxLength(512, ErrorMessage = "Exceeding the max length, allowed only 512 character")]
        public System.String AreanName {get;set;}
        
        [Display(Name ="Area description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String AreaDescription {get;set;}
        
        [Display(Name ="Directorate id" )]
        [Required(ErrorMessage = "Directorate id is required")]
        public System.Int32 DirectorateId {get;set;}
        
        [Display(Name ="Area custom class" )]
        [MaxLength(123, ErrorMessage = "Exceeding the max length, allowed only 123 character")]
        public System.String AreaCustomClass {get;set;}
        
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
        
        [Display(Name ="Status value" )]
        public object StatusValue {get;set;}
        
        [Display(Name ="Units" )]
        public object Units {get;set;}
  

    }
}


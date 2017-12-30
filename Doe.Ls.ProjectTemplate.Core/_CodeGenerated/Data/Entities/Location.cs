


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(LocationMetadata))]
    public partial class Location
    {
        public override string ToString()
        {
            return string.Format("{0}", this.Name);
        }
    }

    public class LocationMetadata
    {
        
        [Display(Name ="Location id" )]
        [Required(ErrorMessage = "Location id is required")]
        public System.Int32 LocationId {get;set;}
        
        [Display(Name ="Name" )]
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150, ErrorMessage = "Exceeding the max length, allowed only 150 character")]
        public System.String Name {get;set;}
        
        [Display(Name ="Directorate id" )]
        [Required(ErrorMessage = "Directorate id is required")]
        public System.Int32 DirectorateId {get;set;}
        
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
        
        [Display(Name ="Positions" )]
        public object Positions {get;set;}
  

    }
}


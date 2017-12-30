


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(RoleCapabilityMetadata))]
    public partial class RoleCapability
    {
        public override string ToString()
        {
            return $"{this.CreatedBy}";
        }
    }

    public class RoleCapabilityMetadata
    {
        
        [Display(Name ="Role description id" )]
        [Required(ErrorMessage = "Role description id is required")]
        public System.Int32 RoleDescriptionId {get;set;}
        
        [Display(Name ="Capability name id" )]
        [Required(ErrorMessage = "Capability name id is required")]
        public System.Int32 CapabilityNameId {get;set;}
        
        [Display(Name ="Capability level id" )]
        [Required(ErrorMessage = "Capability level id is required")]
        public System.Int32 CapabilityLevelId {get;set;}
        
        [Display(Name ="Highlighted" )]
        [Required(ErrorMessage = "Highlighted is required")]
        public System.Boolean Highlighted {get;set;}
        
        [Display(Name ="Date created" )]
        [Required(ErrorMessage = "Date created is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime DateCreated {get;set;}
        
        [Display(Name ="Last updated" )]
        [Required(ErrorMessage = "Last updated is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime LastUpdated {get;set;}
        
        [Display(Name ="Created by" )]
        [Required(ErrorMessage = "Created by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String CreatedBy {get;set;}
        
        [Display(Name ="Last modified by" )]
        [Required(ErrorMessage = "Last modified by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String LastModifiedBy {get;set;}
  



        
        [Display(Name ="Capability level" )]
        public object CapabilityLevel {get;set;}
        
        [Display(Name ="Capability name" )]
        public object CapabilityName {get;set;}
        
        [Display(Name ="Role description" )]
        public object RoleDescription {get;set;}
  

    }
}

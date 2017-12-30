


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(CapabilityNameMetadata))]
    public partial class CapabilityName
    {
        public override string ToString()
        {
            return $"{this.Name}";
        }
    }

    public class CapabilityNameMetadata
    {
        
        [Display(Name ="Capability name id" )]
        public System.Int32 CapabilityNameId {get;set;}
        
        [Display(Name ="Name" )]
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150, ErrorMessage = "Exceeding the max length, allowed only 150 character")]
        public System.String Name {get;set;}
        
        [Display(Name ="Capability description" )]
        [MaxLength(500, ErrorMessage = "Exceeding the max length, allowed only 500 character")]
        public System.String CapabilityDescription {get;set;}
        
        [Display(Name ="Capability group id" )]
        [Required(ErrorMessage = "Capability group id is required")]
        public System.Int32 CapabilityGroupId {get;set;}
        
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
  



        
        [Display(Name ="Capability behavioural indicators" )]
        public object CapabilityBehaviourIndicators {get;set;}
        
        [Display(Name ="Capability group" )]
        public object CapabilityGroup {get;set;}
        
        [Display(Name ="Role capabilities" )]
        public object RoleCapabilities {get;set;}
  

    }
}


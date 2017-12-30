


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(CapabilityLevelMetadata))]
    public partial class CapabilityLevel
    {
        public override string ToString()
        {
            return string.Format("{0}", this.LevelName);
        }
    }

    public class CapabilityLevelMetadata
    {
        
        [Display(Name ="Capability level id" )]
        public System.Int32 CapabilityLevelId {get;set;}
        
        [Display(Name ="Level name" )]
        [Required(ErrorMessage = "Level name is required")]
        [MaxLength(150, ErrorMessage = "Exceeding the max length, allowed only 150 character")]
        public System.String LevelName {get;set;}
        
        [Display(Name ="Display order" )]
        [Required(ErrorMessage = "Display order is required")]
        public System.Int32 DisplayOrder {get;set;}
        
        [Display(Name ="Level order" )]
        [Required(ErrorMessage = "Level order is required")]
        public System.Int32 LevelOrder {get;set;}
        
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
  



        
        [Display(Name ="Capability behaviour indicators" )]
        public object CapabilityBehaviourIndicators {get;set;}
        
        [Display(Name ="Role capabilities" )]
        public object RoleCapabilities {get;set;}
  

    }
}


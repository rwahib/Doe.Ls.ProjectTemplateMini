


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(CapabilityGroupMetadata))]
    public partial class CapabilityGroup
    {
        public override string ToString()
        {
            return $"{this.GroupName}";
        }
    }

    public class CapabilityGroupMetadata
    {
        
        [Display(Name ="Capability group id" )]
        public System.Int32 CapabilityGroupId {get;set;}
        
        [Display(Name ="Group name" )]
        [Required(ErrorMessage = "Group name is required")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String GroupName {get;set;}
        
        [Display(Name ="Group description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String GroupDescription {get;set;}
        
        [Display(Name ="Display order" )]
        public System.Int32 DisplayOrder {get;set;}
        
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
        
        [Display(Name ="Group image" )]
        [MaxLength(150, ErrorMessage = "Exceeding the max length, allowed only 150 character")]
        public System.String GroupImage {get;set;}
  



        
        [Display(Name ="Capability names" )]
        public object CapabilityNames {get;set;}
  

    }
}


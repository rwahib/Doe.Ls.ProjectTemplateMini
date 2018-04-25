 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(AppEntityTypeMetadata))]
    public partial class AppEntityType
    {
        public override string ToString()
        {
            return string.Format("{0}", this.EntityApiName);
        }
    }

    public class AppEntityTypeMetadata
    {
        
        [Display(Name ="Entity type id" )]
        [Required(ErrorMessage = "Entity type id is required")]
        public System.Int32 EntityTypeId {get;set;}
        
        [Display(Name ="Entity api name" )]
        [Required(ErrorMessage = "Entity api name is required")]
        [MaxLength(240, ErrorMessage = "Exceeding the max length, allowed only 240 character")]
        public System.String EntityApiName {get;set;}
        
        [Display(Name ="Entity title" )]
        [Required(ErrorMessage = "Entity title is required")]
        [MaxLength(240, ErrorMessage = "Exceeding the max length, allowed only 240 character")]
        public System.String EntityTitle {get;set;}
        
        [Display(Name ="Entity description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String EntityDescription {get;set;}
        
        [Display(Name ="Sys admin dashboard" )]
        [Required(ErrorMessage = "Sys admin dashboard is required")]
        public System.Boolean SysAdminDashboard {get;set;}
        
        [Display(Name ="Power user dashboard" )]
        [Required(ErrorMessage = "Power user dashboard is required")]
        public System.Boolean PowerUserDashboard {get;set;}
        
        [Display(Name ="High priority" )]
        [Required(ErrorMessage = "High priority is required")]
        public System.Boolean HighPriority {get;set;}
  



  

    }
}


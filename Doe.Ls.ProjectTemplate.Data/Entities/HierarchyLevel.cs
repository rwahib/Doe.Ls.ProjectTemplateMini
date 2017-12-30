


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(HierarchyLevelMetadata))]
    public partial class HierarchyLevel
    {
        public override string ToString()
        {
            return $"{this.HierarchyName}";
        }
    }

    public class HierarchyLevelMetadata
    {
        
        [Display(Name ="Hierarchy id" )]
        [Required(ErrorMessage = "Hierarchy id is required")]
        public System.Int32 HierarchyId {get;set;}
        
        [Display(Name ="Hierarchy name" )]
        [Required(ErrorMessage = "Hierarchy name is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String HierarchyName {get;set;}
        
        [Display(Name ="Hierarchy description" )]
        [MaxLength(240, ErrorMessage = "Exceeding the max length, allowed only 240 character")]
        public System.String HierarchyDescription {get;set;}
  



        
        [Display(Name ="Business units" )]
        public object BusinessUnits {get;set;}
        
        [Display(Name ="Teams" )]
        public object Units {get;set;}
  

    }
}


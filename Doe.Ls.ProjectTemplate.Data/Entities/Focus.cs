


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(FocusMetadata))]
    public partial class Focus
    {
        public override string ToString()
        {
            return $"{this.FocusName}";
        }
    }

    public class FocusMetadata
    {
        
        [Display(Name ="Focus id" )]
        public System.Int32 FocusId {get;set;}
        
        [Display(Name ="Focus name" )]
        [Required(ErrorMessage = "Focus name is required")]
        [MaxLength(150, ErrorMessage = "Exceeding the max length, allowed only 150 character")]
        public System.String FocusName {get;set;}
        
        [Display(Name ="Display order" )]
        [Required(ErrorMessage = "Order list is required")]
        public System.Int32 OrderList {get;set;}
       
        [Display(Name ="Lookup focus grade criteria" )]
        public object LookupFocusGradeCriterias {get;set;}
  

    }
}


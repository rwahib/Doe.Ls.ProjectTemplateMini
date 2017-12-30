


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(RoleDescCapabilityMatrixMetadata))]
    public partial class RoleDescCapabilityMatrix
    {
        public override string ToString()
        {
            return string.Format("{0}", this.GradeCode);
        }
    }

    public class RoleDescCapabilityMatrixMetadata
    {
        
        [Display(Name ="Grade code" )]
        [Required(ErrorMessage = "Grade code is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String GradeCode {get;set;}
        
        [Display(Name ="Foundational  min" )]
        [Required(ErrorMessage = "Foundational  min is required")]
        public System.Int32 Foundational_Min {get;set;}
        
        [Display(Name ="Foundational  max" )]
        [Required(ErrorMessage = "Foundational  max is required")]
        public System.Int32 Foundational_Max {get;set;}
        
        [Display(Name ="Intermediate  min" )]
        [Required(ErrorMessage = "Intermediate  min is required")]
        public System.Int32 Intermediate_Min {get;set;}
        
        [Display(Name ="Intermediate  max" )]
        [Required(ErrorMessage = "Intermediate  max is required")]
        public System.Int32 Intermediate_Max {get;set;}
        
        [Display(Name ="Adept  min" )]
        [Required(ErrorMessage = "Adept  min is required")]
        public System.Int32 Adept_Min {get;set;}
        
        [Display(Name ="Adept  max" )]
        [Required(ErrorMessage = "Adept  max is required")]
        public System.Int32 Adept_Max {get;set;}
        
        [Display(Name ="Advanced  min" )]
        [Required(ErrorMessage = "Advanced  min is required")]
        public System.Int32 Advanced_Min {get;set;}
        
        [Display(Name ="Advanced  max" )]
        [Required(ErrorMessage = "Advanced  max is required")]
        public System.Int32 Advanced_Max {get;set;}
        
        [Display(Name ="Highly advanced  min" )]
        [Required(ErrorMessage = "Highly advanced  min is required")]
        public System.Int32 HighlyAdvanced_Min {get;set;}
        
        [Display(Name ="Highly advanced  max" )]
        [Required(ErrorMessage = "Highly advanced  max is required")]
        public System.Int32 HighlyAdvanced_Max {get;set;}
        
        [Display(Name ="Focus capabilities  min" )]
        [Required(ErrorMessage = "Focus capabilities  min is required")]
        public System.Int32 FocusCapabilities_Min {get;set;}
        
        [Display(Name ="Focus capabilities  max" )]
        [Required(ErrorMessage = "Focus capabilities  max is required")]
        public System.Int32 FocusCapabilities_Max {get;set;}
        
        [Display(Name ="Notes" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String Notes {get;set;}
  



        
        [Display(Name ="Grade" )]
        public object Grade {get;set;}
  

    }
}


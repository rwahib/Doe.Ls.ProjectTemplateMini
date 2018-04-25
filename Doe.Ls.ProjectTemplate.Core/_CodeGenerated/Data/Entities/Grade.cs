 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(GradeMetadata))]
    public partial class Grade
    {
        public override string ToString()
        {
            return string.Format("{0}", this.GradeTitle);
        }
    }

    public class GradeMetadata
    {
        
        [Display(Name ="Grade code" )]
        [Required(ErrorMessage = "Grade code is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String GradeCode {get;set;}
        
        [Display(Name ="Grade title" )]
        [Required(ErrorMessage = "Grade title is required")]
        [MaxLength(256, ErrorMessage = "Exceeding the max length, allowed only 256 character")]
        public System.String GradeTitle {get;set;}
        
        [Display(Name ="Award" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1024, ErrorMessage = "Exceeding the max length, allowed only 1024 character")]
        public System.String Award {get;set;}
        
        [Display(Name ="Award max rates" )]
        public System.Decimal AwardMaxRates {get;set;}
        
        [Display(Name ="Teaching based" )]
        public System.Boolean TeachingBased {get;set;}
        
        [Display(Name ="Grade type" )]
        [Required(ErrorMessage = "Grade type is required")]
        [MaxLength(32, ErrorMessage = "Exceeding the max length, allowed only 32 character")]
        public System.String GradeType {get;set;}
        
        [Display(Name ="Status id" )]
        [Required(ErrorMessage = "Status id is required")]
        public System.Int32 StatusId {get;set;}
        
        [Display(Name ="Message" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String Message {get;set;}
  



        
        [Display(Name ="Status value" )]
        public object StatusValue {get;set;}
        
        [Display(Name ="Lookup focus grade criterias" )]
        public object LookupFocusGradeCriterias {get;set;}
        
        [Display(Name ="Role desc capability matrix" )]
        public object RoleDescCapabilityMatrix {get;set;}
        
        [Display(Name ="Role position descriptions" )]
        public object RolePositionDescriptions {get;set;}
  

    }
}


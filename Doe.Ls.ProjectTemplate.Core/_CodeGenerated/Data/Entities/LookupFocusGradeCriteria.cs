


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(LookupFocusGradeCriteriaMetadata))]
    public partial class LookupFocusGradeCriteria
    {
        public override string ToString()
        {
            return string.Format("{0}", this.GradeCode);
        }
    }

    public class LookupFocusGradeCriteriaMetadata
    {
        
        [Display(Name ="Lookup id" )]
        public System.Int32 LookupId {get;set;}
        
        [Display(Name ="Focus id" )]
        [Required(ErrorMessage = "Focus id is required")]
        public System.Int32 FocusId {get;set;}
        
        [Display(Name ="Grade code" )]
        [Required(ErrorMessage = "Grade code is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String GradeCode {get;set;}
        
        [Display(Name ="Selection criteria id" )]
        [Required(ErrorMessage = "Selection criteria id is required")]
        public System.Int32 SelectionCriteriaId {get;set;}
        
        [Display(Name ="Last updated date" )]
        [Required(ErrorMessage = "Last updated date is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime LastUpdatedDate {get;set;}
        
        [Display(Name ="Last updated by" )]
        [Required(ErrorMessage = "Last updated by is required")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String LastUpdatedBy {get;set;}
        
        [Display(Name ="Is mandatory" )]
        [Required(ErrorMessage = "Is mandatory is required")]
        public System.Boolean IsMandatory {get;set;}
  



        
        [Display(Name ="Focus" )]
        public object Focus {get;set;}
        
        [Display(Name ="Grade" )]
        public object Grade {get;set;}
        
        [Display(Name ="Selection criteria" )]
        public object SelectionCriteria {get;set;}
        
        [Display(Name ="Position focus criterias" )]
        public object PositionFocusCriterias {get;set;}
  

    }
}


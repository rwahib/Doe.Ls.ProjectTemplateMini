


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(SelectionCriteriaMetadata))]
    public partial class SelectionCriteria
    {
        public override string ToString()
        {
            return string.Format("{0}", this.Criteria);
        }
    }

    public class SelectionCriteriaMetadata
    {
        
        [Display(Name ="Selection criteria id" )]
        [Required(ErrorMessage = "Selection criteria id is required")]
        public System.Int32 SelectionCriteriaId {get;set;}
        
        [Display(Name ="Criteria" )]
        [Required(ErrorMessage = "Criteria is required")]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String Criteria {get;set;}
        
        [Display(Name ="Last modified date" )]
        [Required(ErrorMessage = "Last modified date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}")]
        [DataType(DataType.DateTime)]
        public System.DateTime LastModifiedDate {get;set;}
        
        [Display(Name ="Last modified by" )]
        [Required(ErrorMessage = "Last modified by is required")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String LastModifiedBy {get;set;}
  



        
        [Display(Name ="Lookup focus grade criterias" )]
        public object LookupFocusGradeCriterias {get;set;}
  

    }
}


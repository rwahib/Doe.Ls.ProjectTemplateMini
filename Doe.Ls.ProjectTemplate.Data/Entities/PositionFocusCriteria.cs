


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(PositionFocusCriteriaMetadata))]
    public partial class PositionFocusCriteria
    {
        public override string ToString()
        {
            return $"{this.LookupCustomContent}";
        }
    }

    public class PositionFocusCriteriaMetadata
    {
        
        [Display(Name ="Position description id" )]
        [Required(ErrorMessage = "Position description id is required")]
        public System.Int32 PositionDescriptionId {get;set;}
        
        [Display(Name ="Lookup id" )]
        [Required(ErrorMessage = "Lookup id is required")]
        public System.Int32 LookupId {get;set;}
        
        [Display(Name ="Lookup custom content" )]
        [MaxLength(500, ErrorMessage = "Exceeding the max length, allowed only 500 character")]
        public System.String LookupCustomContent {get;set;}
        
        [Display(Name ="Last modified by" )]
        [Required(ErrorMessage = "Last modified by is required")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String LastModifiedBy {get;set;}
        
        [Display(Name ="Last modified date" )]
        [Required(ErrorMessage = "Last modified date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}")]
        [DataType(DataType.DateTime)]
        public System.DateTime LastModifiedDate {get;set;}
  



        
        [Display(Name ="Lookup focus grade criteria" )]
        public object LookupFocusGradeCriteria {get;set;}
        
        [Display(Name ="Position description" )]
        public object PositionDescription {get;set;}
  

    }
}


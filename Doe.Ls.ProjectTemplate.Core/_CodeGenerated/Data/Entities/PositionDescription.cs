 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(PositionDescriptionMetadata))]
    public partial class PositionDescription
    {
        public override string ToString()
        {
            return string.Format("{0}", this.BriefRoleStatement);
        }
    }

    public class PositionDescriptionMetadata
    {
        
        [Display(Name ="Position description id" )]
        [Required(ErrorMessage = "Position description id is required")]
        public System.Int32 PositionDescriptionId {get;set;}
        
        [Display(Name ="Brief role statement" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String BriefRoleStatement {get;set;}
        
        [Display(Name ="Statement of duties" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String StatementOfDuties {get;set;}
  



        
        [Display(Name ="Role position description" )]
        public object RolePositionDescription {get;set;}
        
        [Display(Name ="Position focus criterias" )]
        public object PositionFocusCriterias {get;set;}
  

    }
}


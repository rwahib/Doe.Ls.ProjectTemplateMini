 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(PositionTypeMetadata))]
    public partial class PositionType
    {
        public override string ToString()
        {
            return string.Format("{0}", this.PositionTypeName);
        }
    }

    public class PositionTypeMetadata
    {
        
        [Display(Name ="Position type code" )]
        [Required(ErrorMessage = "Position type code is required")]
        [MaxLength(8, ErrorMessage = "Exceeding the max length, allowed only 8 character")]
        public System.String PositionTypeCode {get;set;}
        
        [Display(Name ="Position type name" )]
        [Required(ErrorMessage = "Position type name is required")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String PositionTypeName {get;set;}
        
        [Display(Name ="Position type description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String PositionTypeDescription {get;set;}
  



        
        [Display(Name ="Position informations" )]
        public object PositionInformations {get;set;}
  

    }
}


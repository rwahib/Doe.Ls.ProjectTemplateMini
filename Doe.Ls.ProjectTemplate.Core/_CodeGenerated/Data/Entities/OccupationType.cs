 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(OccupationTypeMetadata))]
    public partial class OccupationType
    {
        public override string ToString()
        {
            return string.Format("{0}", this.OccupationTypeName);
        }
    }

    public class OccupationTypeMetadata
    {
        
        [Display(Name ="Occupation type code" )]
        [Required(ErrorMessage = "Occupation type code is required")]
        [MaxLength(12, ErrorMessage = "Exceeding the max length, allowed only 12 character")]
        public System.String OccupationTypeCode {get;set;}
        
        [Display(Name ="Occupation type name" )]
        [Required(ErrorMessage = "Occupation type name is required")]
        [MaxLength(64, ErrorMessage = "Exceeding the max length, allowed only 64 character")]
        public System.String OccupationTypeName {get;set;}
  



        
        [Display(Name ="Position informations" )]
        public object PositionInformations {get;set;}
  

    }
}


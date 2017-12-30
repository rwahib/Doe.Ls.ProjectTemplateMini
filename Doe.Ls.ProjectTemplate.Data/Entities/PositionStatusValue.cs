


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(PositionStatusValueMetadata))]
    public partial class PositionStatusValue
    {
        public override string ToString()
        {
            return $"{this.PosStatusTitle}";
        }
    }

    public class PositionStatusValueMetadata
    {
        
        [Display(Name ="Pos status code" )]
        [Required(ErrorMessage = "Pos status code is required")]
        [MaxLength(12, ErrorMessage = "Exceeding the max length, allowed only 12 character")]
        public System.String PosStatusCode {get;set;}
        
        [Display(Name ="Pos status title" )]
        [Required(ErrorMessage = "Pos status title is required")]
        [MaxLength(64, ErrorMessage = "Exceeding the max length, allowed only 64 character")]
        public System.String PosStatusTitle {get;set;}
  



  

    }
}


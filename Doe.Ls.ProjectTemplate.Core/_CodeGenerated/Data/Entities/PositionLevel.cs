 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(PositionLevelMetadata))]
    public partial class PositionLevel
    {
        public override string ToString()
        {
            return string.Format("{0}", this.PositionLevelName);
        }
    }

    public class PositionLevelMetadata
    {
        
        [Display(Name ="Position level id" )]
        [Required(ErrorMessage = "Position level id is required")]
        public System.Int32 PositionLevelId {get;set;}
        
        [Display(Name ="Position level name" )]
        [Required(ErrorMessage = "Position level name is required")]
        [MaxLength(240, ErrorMessage = "Exceeding the max length, allowed only 240 character")]
        public System.String PositionLevelName {get;set;}
        
        [Display(Name ="Custom class" )]
        [MaxLength(123, ErrorMessage = "Exceeding the max length, allowed only 123 character")]
        public System.String CustomClass {get;set;}
  



        
        [Display(Name ="Positions" )]
        public object Positions {get;set;}
  

    }
}


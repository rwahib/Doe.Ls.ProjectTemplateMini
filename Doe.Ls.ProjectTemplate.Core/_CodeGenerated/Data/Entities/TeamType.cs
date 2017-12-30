


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(TeamTypeMetadata))]
    public partial class TeamType
    {
        public override string ToString()
        {
            return string.Format("{0}", this.TeamTypeName);
        }
    }

    public class TeamTypeMetadata
    {
        
        [Display(Name ="Team type id" )]
        [Required(ErrorMessage = "Team type id is required")]
        public System.Int32 TeamTypeId {get;set;}
        
        [Display(Name ="Team type name" )]
        [Required(ErrorMessage = "Team type name is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String TeamTypeName {get;set;}
        
        [Display(Name ="Team type description" )]
        [MaxLength(240, ErrorMessage = "Exceeding the max length, allowed only 240 character")]
        public System.String TeamTypeDescription {get;set;}
  



        
        [Display(Name ="Units" )]
        public object Units {get;set;}
  

    }
}


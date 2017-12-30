


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(OrgLevelMetadata))]
    public partial class OrgLevel
    {
        public override string ToString()
        {
            return $"{this.OrgLevelName}";
        }
    }

    public class OrgLevelMetadata
    {
        
        [Display(Name ="Org level id" )]
        [Required(ErrorMessage = "Org level id is required")]
        public System.Int32 OrgLevelId {get;set;}
        
        [Display(Name ="Org level title" )]
        [Required(ErrorMessage = "Org level title is required")]
        [MaxLength(240, ErrorMessage = "Exceeding the max length, allowed only 240 character")]
        public System.String OrgLevelTitle {get;set;}
        
        [Display(Name ="Org level name" )]
        [Required(ErrorMessage = "Org level name is required")]
        [MaxLength(240, ErrorMessage = "Exceeding the max length, allowed only 240 character")]
        public System.String OrgLevelName {get;set;}
        
        [Display(Name ="Description" )]
        [MaxLength(255, ErrorMessage = "Exceeding the max length, allowed only 255 character")]
        public System.String Description {get;set;}
  



        
        [Display(Name ="Sys user roles" )]
        public object SysUserRoles {get;set;}
  

    }
}


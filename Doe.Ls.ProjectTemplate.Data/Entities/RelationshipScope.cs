


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(RelationshipScopeMetadata))]
    public partial class RelationshipScope
    {
        public override string ToString()
        {
            return $"{this.ScopeTitle}";
        }
    }

    public class RelationshipScopeMetadata
    {
        
        [Display(Name ="Scope id" )]
        [Required(ErrorMessage = "Scope id is required")]
        public System.Int32 ScopeId {get;set;}
        
        [Display(Name ="Scope title" )]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String ScopeTitle {get;set;}
  



        
        [Display(Name ="Key relationships" )]
        public object KeyRelationships {get;set;}
  

    }
}


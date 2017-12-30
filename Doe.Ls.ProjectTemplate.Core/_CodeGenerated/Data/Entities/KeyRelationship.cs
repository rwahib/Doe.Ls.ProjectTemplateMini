


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(KeyRelationshipMetadata))]
    public partial class KeyRelationship
    {
        public override string ToString()
        {
            return string.Format("{0}", this.ModifiedUserName);
        }
    }

    public class KeyRelationshipMetadata
    {
        
        [Display(Name ="Key relationship id" )]
        public System.Int32 KeyRelationshipId {get;set;}
        
        [Display(Name ="Role description id" )]
        [Required(ErrorMessage = "Role description id is required")]
        public System.Int32 RoleDescriptionId {get;set;}
        
        [Display(Name ="Scope id" )]
        public System.Int32 ScopeId {get;set;}
        
        [Display(Name ="Order number" )]
        public System.Int32 OrderNumber {get;set;}
        
        [Display(Name ="Who" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String Who {get;set;}
        
        [Display(Name ="Why" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String Why {get;set;}
        
        [Display(Name ="Date created" )]
        [Required(ErrorMessage = "Date created is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime DateCreated {get;set;}
        
        [Display(Name ="Modified user name" )]
        [Required(ErrorMessage = "Modified user name is required")]
        [MaxLength(250, ErrorMessage = "Exceeding the max length, allowed only 250 character")]
        public System.String ModifiedUserName {get;set;}
        
        [Display(Name ="Last updated" )]
        [Required(ErrorMessage = "Last updated is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime LastUpdated {get;set;}
  



        
        [Display(Name ="Relationship scope" )]
        public object RelationshipScope {get;set;}
        
        [Display(Name ="Role description" )]
        public object RoleDescription {get;set;}
  

    }
}


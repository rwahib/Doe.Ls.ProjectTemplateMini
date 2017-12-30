


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(StatusValueMetadata))]
    public partial class StatusValue
    {
        public override string ToString()
        {
            return $"{this.StatusName}";
        }
        
    }

    public class StatusValueMetadata
    {
        
        [Display(Name ="Status id" )]
        [Required(ErrorMessage = "Status id is required")]
        public System.Int32 StatusId {get;set;}
        
        [Display(Name ="Status name" )]
        [MaxLength(256, ErrorMessage = "Exceeding the max length, allowed only 256 character")]
        public System.String StatusName {get;set;}
        
        [Display(Name ="Status description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1024, ErrorMessage = "Exceeding the max length, allowed only 1024 character")]
        public System.String StatusDescription {get;set;}
  
        [Display(Name ="Directorates" )]
        public object Directorates {get;set;}
        
        [Display(Name ="Divisions" )]
        public object Executives {get;set;}
        
        [Display(Name ="Functional areas" )]
        public object FunctionalAreas {get;set;}
        
        [Display(Name ="Grades" )]
        public object Grades {get;set;}
        
        [Display(Name ="Positions" )]
        public object Positions {get;set;}
        
        [Display(Name ="Units" )]
        public object Units {get;set;}
  

    }
}


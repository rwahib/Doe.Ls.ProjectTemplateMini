


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(TrimRecordMetadata))]
    public partial class TrimRecord
    {
        public override string ToString()
        {
            return string.Format("{0}", this.Token);
        }
    }

    public class TrimRecordMetadata
    {
        
        [Display(Name ="Role position desc id" )]
        [Required(ErrorMessage = "Role position desc id is required")]
        public System.Int32 RolePositionDescId {get;set;}
        
        [Display(Name ="Uri" )]
        public System.Int64 Uri {get;set;}
        
        [Display(Name ="Token" )]
        [MaxLength(64, ErrorMessage = "Exceeding the max length, allowed only 64 character")]
        public System.String Token {get;set;}
        
        [Display(Name ="Last published date" )]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime LastPublishedDate {get;set;}
        
        [Display(Name ="Last revision number" )]
        public System.Int32 LastRevisionNumber {get;set;}
  



        
        [Display(Name ="Role position description" )]
        public object RolePositionDescription {get;set;}
  

    }
}


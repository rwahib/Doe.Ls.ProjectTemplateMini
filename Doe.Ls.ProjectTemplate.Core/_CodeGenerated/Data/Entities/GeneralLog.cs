 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(GeneralLogMetadata))]
    public partial class GeneralLog
    {
        public override string ToString()
        {
            return string.Format("{0}", this.Username);
        }
    }

    public class GeneralLogMetadata
    {
        
        [Display(Name ="Log id" )]
        public System.Int32 LogId {get;set;}
        
        [Display(Name ="Action" )]
        [MaxLength(128, ErrorMessage = "Exceeding the max length, allowed only 128 character")]
        public System.String Action {get;set;}
        
        [Display(Name ="Context" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String Context {get;set;}
        
        [Display(Name ="Creation date" )]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime CreationDate {get;set;}
        
        [Display(Name ="Username" )]
        [MaxLength(128, ErrorMessage = "Exceeding the max length, allowed only 128 character")]
        public System.String Username {get;set;}
        
        [Display(Name ="Role id" )]
        [Required(ErrorMessage = "Role id is required")]
        public System.Int32 RoleId {get;set;}
        
        [Display(Name ="Note" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String Note {get;set;}
  



        
        [Display(Name ="Sys role" )]
        public object SysRole {get;set;}
  

    }
}


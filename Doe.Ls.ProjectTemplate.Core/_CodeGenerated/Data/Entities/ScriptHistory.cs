


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(ScriptHistoryMetadata))]
    public partial class ScriptHistory
    {
        public override string ToString()
        {
            return string.Format("{0}", this.ScriptName);
        }
    }

    public class ScriptHistoryMetadata
    {
        
        [Display(Name ="Script number" )]
        [Required(ErrorMessage = "Script number is required")]
        [MaxLength(12, ErrorMessage = "Exceeding the max length, allowed only 12 character")]
        public System.String ScriptNumber {get;set;}
        
        [Display(Name ="Script name" )]
        [Required(ErrorMessage = "Script name is required")]
        [MaxLength(256, ErrorMessage = "Exceeding the max length, allowed only 256 character")]
        public System.String ScriptName {get;set;}
        
        [Display(Name ="Run date" )]
        [Required(ErrorMessage = "Run date is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime RunDate {get;set;}
        
        [Display(Name ="Run by" )]
        [Required(ErrorMessage = "Run by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String RunBy {get;set;}
        
        [Display(Name ="Comments" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1000, ErrorMessage = "Exceeding the max length, allowed only 1000 character")]
        public System.String Comments {get;set;}
  



  

    }
}


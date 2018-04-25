 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(SysMessageMetadata))]
    public partial class SysMessage
    {
        public override string ToString()
        {
            return string.Format("{0}", this.Code);
        }
    }

    public class SysMessageMetadata
    {
        
        [Display(Name ="Code" )]
        [Required(ErrorMessage = "Code is required")]
        [MaxLength(64, ErrorMessage = "Exceeding the max length, allowed only 64 character")]
        public System.String Code {get;set;}
        
        [Display(Name ="Message format" )]
        [Required(ErrorMessage = "Message format is required")]
        [MaxLength(500, ErrorMessage = "Exceeding the max length, allowed only 500 character")]
        public System.String MessageFormat {get;set;}
        
        [Display(Name ="Msg category id" )]
        [Required(ErrorMessage = "Msg category id is required")]
        public System.Int32 MsgCategoryId {get;set;}
        
        [Display(Name ="Message hint" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String MessageHint {get;set;}
        
        [Display(Name ="Created date" )]
        [Required(ErrorMessage = "Created date is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime CreatedDate {get;set;}
        
        [Display(Name ="Last modified date" )]
        [Required(ErrorMessage = "Last modified date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}")]
        [DataType(DataType.DateTime)]
        public System.DateTime LastModifiedDate {get;set;}
        
        [Display(Name ="Created by" )]
        [Required(ErrorMessage = "Created by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String CreatedBy {get;set;}
        
        [Display(Name ="Last modified by" )]
        [Required(ErrorMessage = "Last modified by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String LastModifiedBy {get;set;}
  



        
        [Display(Name ="Sys msg category" )]
        public object SysMsgCategory {get;set;}
  

    }
}


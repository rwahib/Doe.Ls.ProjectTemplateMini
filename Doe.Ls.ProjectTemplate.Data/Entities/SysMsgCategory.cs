


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(SysMsgCategoryMetadata))]
    public partial class SysMsgCategory
    {
        public override string ToString()
        {
            return $"{MsgCategoryId}-{this.MsgCategoryName}";
        }
    }

    public class SysMsgCategoryMetadata
    {
        
        [Display(Name ="Message category id" )]
        [Required(ErrorMessage = "Message category id is required")]
        public System.Int32 MsgCategoryId {get;set;}
        
        [Display(Name = "Message category name")]
        [Required(ErrorMessage = "Message category name is required")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String MsgCategoryName {get;set;}
  



        
        [Display(Name ="Messages" )]
        public object SysMessages {get;set;}
  

    }
}


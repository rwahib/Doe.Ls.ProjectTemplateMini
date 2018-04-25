 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(SysMsgCategoryMetadata))]
    public partial class SysMsgCategory
    {
        public override string ToString()
        {
            return string.Format("{0}", this.MsgCategoryName);
        }
    }

    public class SysMsgCategoryMetadata
    {
        
        [Display(Name ="Msg category id" )]
        [Required(ErrorMessage = "Msg category id is required")]
        public System.Int32 MsgCategoryId {get;set;}
        
        [Display(Name ="Msg category name" )]
        [Required(ErrorMessage = "Msg category name is required")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String MsgCategoryName {get;set;}
  



        
        [Display(Name ="Sys messages" )]
        public object SysMessages {get;set;}
  

    }
}


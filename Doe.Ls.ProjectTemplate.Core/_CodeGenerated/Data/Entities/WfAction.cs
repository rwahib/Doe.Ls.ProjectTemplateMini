


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(WfActionMetadata))]
    public partial class WfAction
    {
        public override string ToString()
        {
            return string.Format("{0}", this.WfActionName);
        }
    }

    public class WfActionMetadata
    {
        
        [Display(Name ="Wf action id" )]
        [Required(ErrorMessage = "Wf action id is required")]
        public System.Int32 WfActionId {get;set;}
        
        [Display(Name ="Wf action name" )]
        [Required(ErrorMessage = "Wf action name is required")]
        [MaxLength(128, ErrorMessage = "Exceeding the max length, allowed only 128 character")]
        public System.String WfActionName {get;set;}
        
        [Display(Name ="Wf action status" )]
        [MaxLength(128, ErrorMessage = "Exceeding the max length, allowed only 128 character")]
        public System.String WfActionStatus {get;set;}
        
        [Display(Name ="Wf action description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String WfActionDescription {get;set;}
  



  

    }
}


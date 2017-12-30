


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(WfActionMetadata))]
    public partial class WfAction
    {
        public override string ToString()
        {
            return $"{this.WfActionId}-{this.WfActionName}-{WfActionStatus}";
        }
    }

    public class WfActionMetadata
    {
        
        [Display(Name ="Workflow action id" )]
        [Required(ErrorMessage = "Workflow action id is required")]
        public System.Int32 WfActionId {get;set;}
        
        [Display(Name ="Workflow action name" )]
        [Required(ErrorMessage = "Workflow action name is required")]
        [MaxLength(128, ErrorMessage = "Exceeding the max length, allowed only 128 character")]
        public System.String WfActionName {get;set;}
        
        [Display(Name ="Workflow action status" )]
        [MaxLength(128, ErrorMessage = "Exceeding the max length, allowed only 128 character")]
        public System.String WfActionStatus {get;set;}
        
        [Display(Name ="Workflow action description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String WfActionDescription {get;set;}
  

  

    }
}


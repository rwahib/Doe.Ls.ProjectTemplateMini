


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(GlobalSettingMetadata))]
    public partial class GlobalSetting
    {
        public override string ToString()
        {
            return string.Format("{0}", this.PropertyCode);
        }
    }

    public class GlobalSettingMetadata
    {
        
        [Display(Name ="Settings key" )]
        [Required(ErrorMessage = "Settings key is required")]
        public System.Int32 SettingsKey {get;set;}
        
        [Display(Name ="Property code" )]
        [Required(ErrorMessage = "Property code is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String PropertyCode {get;set;}
        
        [Display(Name ="Property boolean value" )]
        public System.Boolean PropertyBooleanValue {get;set;}
        
        [Display(Name ="Property value" )]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String PropertyValue {get;set;}
        
        [Display(Name ="Entity context code" )]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String EntityContextCode {get;set;}
        
        [Display(Name ="Entity context value" )]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String EntityContextValue {get;set;}
        
        [Display(Name ="Tag" )]
        [MaxLength(500, ErrorMessage = "Exceeding the max length, allowed only 500 character")]
        public System.String Tag {get;set;}
  



  

    }
}


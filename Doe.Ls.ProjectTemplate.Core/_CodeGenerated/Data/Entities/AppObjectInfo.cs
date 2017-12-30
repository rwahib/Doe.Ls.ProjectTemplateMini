


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(AppObjectInfoMetadata))]
    public partial class AppObjectInfo
    {
        public override string ToString()
        {
            return string.Format("{0}", this.ObjectName);
        }
    }

    public class AppObjectInfoMetadata
    {
        
        [Display(Name ="Object name" )]
        [Required(ErrorMessage = "Object name is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String ObjectName {get;set;}
        
        [Display(Name ="Counter value" )]
        public System.Int32 CounterValue {get;set;}
        
        [Display(Name ="Aggregated value a" )]
        public System.Double AggregatedValueA {get;set;}
        
        [Display(Name ="Aggregated value b" )]
        public System.Double AggregatedValueB {get;set;}
        
        [Display(Name ="Metadata" )]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String Metadata {get;set;}
        
        [Display(Name ="Last modified date" )]
        [Required(ErrorMessage = "Last modified date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}")]
        [DataType(DataType.DateTime)]
        public System.DateTime LastModifiedDate {get;set;}
        
        [Display(Name ="last modified user" )]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String lastModifiedUser {get;set;}
  



  

    }
}


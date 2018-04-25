 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(GlobalItemMetadata))]
    public partial class GlobalItem
    {
        public override string ToString()
        {
            return string.Format("{0}", this.ItemName);
        }
    }

    public class GlobalItemMetadata
    {
        
        [Display(Name ="Item code" )]
        [Required(ErrorMessage = "Item code is required")]
        [MaxLength(12, ErrorMessage = "Exceeding the max length, allowed only 12 character")]
        public System.String ItemCode {get;set;}
        
        [Display(Name ="Item name" )]
        [Required(ErrorMessage = "Item name is required")]
        [MaxLength(124, ErrorMessage = "Exceeding the max length, allowed only 124 character")]
        public System.String ItemName {get;set;}
        
        [Display(Name ="Item description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String ItemDescription {get;set;}
        
        [Display(Name ="Item content" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String ItemContent {get;set;}
        
        [Display(Name ="Created by" )]
        [Required(ErrorMessage = "Created by is required")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String CreatedBy {get;set;}
        
        [Display(Name ="Created date" )]
        [Required(ErrorMessage = "Created date is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime CreatedDate {get;set;}
        
        [Display(Name ="Lastupdated by" )]
        [Required(ErrorMessage = "Lastupdated by is required")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String LastupdatedBy {get;set;}
        
        [Display(Name ="Lastupdated date" )]
        [Required(ErrorMessage = "Lastupdated date is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime LastupdatedDate {get;set;}
  



  

    }
}


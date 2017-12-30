


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(PositionHistoryMetadata))]
    public partial class PositionHistory
    {
        public override string ToString()
        {
            return $"{this.Action}";
        }
    }

    public class PositionHistoryMetadata
    {
        
        [Display(Name ="Position history id" )]
        public System.Int32 PositionHistoryId {get;set;}
        
        [Display(Name ="Position id" )]
        [Required(ErrorMessage = "Position id is required")]
        public System.Int32 PositionId {get;set;}
        
        [Display(Name ="Action" )]
        [Required(ErrorMessage = "Action is required")]
        [MaxLength(256, ErrorMessage = "Exceeding the max length, allowed only 256 character")]
        public System.String Action {get;set;}
        
        [Display(Name ="Status from" )]
        [Required(ErrorMessage = "Status from is required")]
        [MaxLength(24, ErrorMessage = "Exceeding the max length, allowed only 24 character")]
        public System.String StatusFrom {get;set;}
        
        [Display(Name ="Status to" )]
        [Required(ErrorMessage = "Status to is required")]
        [MaxLength(24, ErrorMessage = "Exceeding the max length, allowed only 24 character")]
        public System.String StatusTo {get;set;}
        
        [Display(Name ="Additional info" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String AdditionalInfo {get;set;}
        
        [Display(Name ="Created date" )]
        [Required(ErrorMessage = "Created date is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime CreatedDate {get;set;}
        
        [Display(Name ="Created by" )]
        [Required(ErrorMessage = "Created by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String CreatedBy {get;set;}
  



        
        [Display(Name ="Position" )]
        public object Position {get;set;}
  

    }
}


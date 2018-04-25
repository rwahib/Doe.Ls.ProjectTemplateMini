 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(PositionInformationMetadata))]
    public partial class PositionInformation
    {
        public override string ToString()
        {
            return string.Format("{0}", this.OlderPositionNumber3);
        }
    }

    public class PositionInformationMetadata
    {
        
        [Display(Name ="Position id" )]
        [Required(ErrorMessage = "Position id is required")]
        public System.Int32 PositionId {get;set;}
        
        [Display(Name ="Older position number3" )]
        [MaxLength(10, ErrorMessage = "Exceeding the max length, allowed only 10 character")]
        public System.String OlderPositionNumber3 {get;set;}
        
        [Display(Name ="Older position number1" )]
        [MaxLength(10, ErrorMessage = "Exceeding the max length, allowed only 10 character")]
        public System.String OlderPositionNumber1 {get;set;}
        
        [Display(Name ="Older position number2" )]
        [MaxLength(20, ErrorMessage = "Exceeding the max length, allowed only 20 character")]
        public System.String OlderPositionNumber2 {get;set;}
        
        [Display(Name ="Sch number" )]
        [MaxLength(20, ErrorMessage = "Exceeding the max length, allowed only 20 character")]
        public System.String SchNumber {get;set;}
        
        [Display(Name ="Position type code" )]
        [Required(ErrorMessage = "Position type code is required")]
        [MaxLength(8, ErrorMessage = "Exceeding the max length, allowed only 8 character")]
        public System.String PositionTypeCode {get;set;}
        
        [Display(Name ="Employee type code" )]
        [Required(ErrorMessage = "Employee type code is required")]
        [MaxLength(8, ErrorMessage = "Exceeding the max length, allowed only 8 character")]
        public System.String EmployeeTypeCode {get;set;}
        
        [Display(Name ="Position note id" )]
        public System.Int32 PositionNoteId {get;set;}
        
        [Display(Name ="Trim link" )]
        [MaxLength(350, ErrorMessage = "Exceeding the max length, allowed only 350 character")]
        public System.String TrimLink {get;set;}
        
        [Display(Name ="Position end date" )]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime PositionEndDate {get;set;}
        
        [Display(Name ="Position f t e" )]
        [Required(ErrorMessage = "Position f t e is required")]
        public System.Double PositionFTE {get;set;}
        
        [Display(Name ="Position status code" )]
        [Required(ErrorMessage = "Position status code is required")]
        [MaxLength(12, ErrorMessage = "Exceeding the max length, allowed only 12 character")]
        public System.String PositionStatusCode {get;set;}
        
        [Display(Name ="Occupation type code" )]
        [Required(ErrorMessage = "Occupation type code is required")]
        [MaxLength(12, ErrorMessage = "Exceeding the max length, allowed only 12 character")]
        public System.String OccupationTypeCode {get;set;}
        
        [Display(Name ="Other overview" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String OtherOverview {get;set;}
  



        
        [Display(Name ="Employee type" )]
        public object EmployeeType {get;set;}
        
        [Display(Name ="Occupation type" )]
        public object OccupationType {get;set;}
        
        [Display(Name ="Position status value" )]
        public object PositionStatusValue {get;set;}
        
        [Display(Name ="Position type" )]
        public object PositionType {get;set;}
        
        [Display(Name ="Position notes" )]
        public object PositionNotes {get;set;}
        
        [Display(Name ="Position" )]
        public object Position {get;set;}
  

    }
}


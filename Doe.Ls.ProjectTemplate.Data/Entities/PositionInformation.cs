


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(PositionInformationMetadata))]
    public partial class PositionInformation
    {
        public override string ToString()
        {
            return $"{this.OlderPositionNumber3}";
        }
    }

    public class PositionInformationMetadata
    {
        
        [Display(Name ="Position id" )]
        [Required(ErrorMessage = "Position id is required")]
        public System.Int32 PositionId {get;set;}
        
        [Display(Name ="Former position number 3" )]
        [MaxLength(10, ErrorMessage = "Exceeding the max length, allowed only 10 character")]
        public System.String OlderPositionNumber3 {get;set;}
        
        [Display(Name ="Former position number 1" )]
        [MaxLength(10, ErrorMessage = "Exceeding the max length, allowed only 10 character")]
        public System.String OlderPositionNumber1 {get;set;}
        
        [Display(Name ="Former position number 2" )]
        [MaxLength(10, ErrorMessage = "Exceeding the max length, allowed only 10 character")]
        public System.String OlderPositionNumber2 {get;set;}
        
        [Display(Name ="Sch number" )]
        [MaxLength(20, ErrorMessage = "Exceeding the max length, allowed only 20 character")]
        public System.String SchNumber {get;set;}
        
        [Display(Name ="Position type" )]
        [Required(ErrorMessage = "Position type is required")]
        [MaxLength(8, ErrorMessage = "Exceeding the max length, allowed only 8 character")]
        public System.String PositionTypeCode {get;set;}
        
        [Display(Name ="Employee type" )]
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
        
        [Display(Name ="Position FTE" )]
        [Required(ErrorMessage = "Position FTE is required")]
        public System.Double PositionFTE {get;set;}

        [Display(Name = "Position status ")]
        [Required(ErrorMessage = "Position status is required")]
        [MaxLength(12, ErrorMessage = "Exceeding the max length, allowed only 12 character")]
        public System.String PositionStatusCode { get; set; }

        [Display(Name = "Occupation type")]
        [Required(ErrorMessage = "Occupation type is required")]
        [MaxLength(12, ErrorMessage = "Exceeding the max length, allowed only 12 character")]
        public System.String OccupationTypeCode { get; set; }

        [Display(Name = "Additional overview")]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String OtherOverview { get; set; }


        [Display(Name ="Employee type" )]
        public object EmployeeType {get;set;}
        
        [Display(Name ="Position" )]
        public object Position {get;set;}
        
        [Display(Name ="Position type" )]
        public object PositionType {get;set;}
        
        [Display(Name ="Position notes" )]
        public object PositionNotes {get;set;}

        [Display(Name = "Occupation type")]
        public object OccupationType { get; set; }

        [Display(Name = "Position status")]
        public object PositionStatusValue { get; set; }


    }
}


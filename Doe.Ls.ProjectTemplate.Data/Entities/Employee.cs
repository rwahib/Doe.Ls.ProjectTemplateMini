


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
        public override string ToString()
        {
            return $"{this.FirstName}";
        }
    }

    public class EmployeeMetadata
    {
        
        [Display(Name ="Employee id" )]
        public System.Int32 EmployeeId {get;set;}
        
        [Display(Name ="First name" )]
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String FirstName {get;set;}
        
        [Display(Name ="Last name" )]
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String LastName {get;set;}
        
        [Display(Name ="Cost centre number" )]
        public System.Decimal CostCentreNumber {get;set;}
        
        [Display(Name ="Created date" )]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime CreatedDate {get;set;}
        
        [Display(Name ="Last modified date" )]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}")]
        [DataType(DataType.DateTime)]
        public System.DateTime LastModifiedDate {get;set;}
        
        [Display(Name ="Created by" )]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String CreatedBy {get;set;}
        
        [Display(Name ="Last modified by" )]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String LastModifiedBy {get;set;}
        
        [Display(Name ="Status id" )]
        [Required(ErrorMessage = "Status id is required")]
        public System.Int32 StatusId {get;set;}
  



        
        [Display(Name ="Employee positions" )]
        public object EmployeePositions {get;set;}
  

    }
}


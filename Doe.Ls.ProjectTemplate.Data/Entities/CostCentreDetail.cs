


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(CostCentreDetailMetadata))]
    public partial class CostCentreDetail
    {
        public override string ToString()
        {
            return $"{this.CostCentre}";
        }
    }

    public class CostCentreDetailMetadata
    {
        
        [Display(Name ="Position id" )]
        [Required(ErrorMessage = "Position id is required")]
        public System.Int32 PositionId {get;set;}
        
        [Display(Name ="Cost centre code" )]
        [MaxLength(150, ErrorMessage = "Exceeding the max length, allowed only 150 character")]
        public System.String CostCentre {get;set;}
        
        [Display(Name ="Fund number" )]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String Fund {get;set;}
        
        [Display(Name ="Payroll WBS" )]
        [MaxLength(150, ErrorMessage = "Exceeding the max length, allowed only 150 character")]
        public System.String PayrollWBS {get;set;}


        [Display(Name = "RCC JDE Payroll code")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String RCCJDEPayrollCode { get; set; }

        [Display(Name = "GL account")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String GLAccount { get; set; }


        [Display(Name ="Last updated date" )]
        [Required(ErrorMessage = "Last updated date is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime LastUpdatedDate {get;set;}
        
        [Display(Name ="Last updated by" )]
        [Required(ErrorMessage = "Last updated by is required")]
        [MaxLength(250, ErrorMessage = "Exceeding the max length, allowed only 250 character")]
        public System.String LastUpdatedBy {get;set;}
  



        
        [Display(Name ="Position" )]
        public object Position {get;set;}
  

    }
}


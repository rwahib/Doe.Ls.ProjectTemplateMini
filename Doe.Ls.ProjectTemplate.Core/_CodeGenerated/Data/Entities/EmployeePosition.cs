 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(EmployeePositionMetadata))]
    public partial class EmployeePosition
    {
        public override string ToString()
        {
            return string.Format("{0}", this.Reason);
        }
    }

    public class EmployeePositionMetadata
    {
        
        [Display(Name ="Employee id" )]
        [Required(ErrorMessage = "Employee id is required")]
        public System.Int32 EmployeeId {get;set;}
        
        [Display(Name ="Position id" )]
        [Required(ErrorMessage = "Position id is required")]
        public System.Int32 PositionId {get;set;}
        
        [Display(Name ="Status id" )]
        [Required(ErrorMessage = "Status id is required")]
        public System.Int32 StatusId {get;set;}
        
        [Display(Name ="Display in org chart" )]
        [Required(ErrorMessage = "Display in org chart is required")]
        public System.Boolean DisplayInOrgChart {get;set;}
        
        [Display(Name ="Reason" )]
        [MaxLength(250, ErrorMessage = "Exceeding the max length, allowed only 250 character")]
        public System.String Reason {get;set;}
        
        [Display(Name ="From date" )]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime FromDate {get;set;}
        
        [Display(Name ="To date" )]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime ToDate {get;set;}
        
        [Display(Name ="Last modified by" )]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String LastModifiedBy {get;set;}
        
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
  



        
        [Display(Name ="Employee" )]
        public object Employee {get;set;}
        
        [Display(Name ="Position" )]
        public object Position {get;set;}
  

    }
}


 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(EmployeeTypeMetadata))]
    public partial class EmployeeType
    {
        public override string ToString()
        {
            return string.Format("{0}", this.EmployeeTypeName);
        }
    }

    public class EmployeeTypeMetadata
    {
        
        [Display(Name ="Employee type code" )]
        [Required(ErrorMessage = "Employee type code is required")]
        [MaxLength(8, ErrorMessage = "Exceeding the max length, allowed only 8 character")]
        public System.String EmployeeTypeCode {get;set;}
        
        [Display(Name ="Employee type name" )]
        [Required(ErrorMessage = "Employee type name is required")]
        [MaxLength(50, ErrorMessage = "Exceeding the max length, allowed only 50 character")]
        public System.String EmployeeTypeName {get;set;}
        
        [Display(Name ="Employee type description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String EmployeeTypeDescription {get;set;}
  



        
        [Display(Name ="Position informations" )]
        public object PositionInformations {get;set;}
  

    }
}


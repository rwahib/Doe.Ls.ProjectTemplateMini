 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(RolePositionDescriptionMetadata))]
    public partial class RolePositionDescription
    {
        public override string ToString()
        {
            return string.Format("{0}", this.Title);
        }
    }

    public class RolePositionDescriptionMetadata
    {
        
        [Display(Name ="Role position desc id" )]
        [Required(ErrorMessage = "Role position desc id is required")]
        public System.Int32 RolePositionDescId {get;set;}
        
        [Display(Name ="Status id" )]
        [Required(ErrorMessage = "Status id is required")]
        public System.Int32 StatusId {get;set;}
        
        [Display(Name ="Version" )]
        [Required(ErrorMessage = "Version is required")]
        public System.Int32 Version {get;set;}
        
        [Display(Name ="Title" )]
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(250, ErrorMessage = "Exceeding the max length, allowed only 250 character")]
        public System.String Title {get;set;}
        
        [Display(Name ="Doc number" )]
        [Required(ErrorMessage = "Doc number is required")]
        [MaxLength(150, ErrorMessage = "Exceeding the max length, allowed only 150 character")]
        public System.String DocNumber {get;set;}
        
        [Display(Name ="Grade code" )]
        [Required(ErrorMessage = "Grade code is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String GradeCode {get;set;}
        
        [Display(Name ="Date of approval" )]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime DateOfApproval {get;set;}
        
        [Display(Name ="Is position description" )]
        [Required(ErrorMessage = "Is position description is required")]
        public System.Boolean IsPositionDescription {get;set;}
        
        [Display(Name ="Created date" )]
        [Required(ErrorMessage = "Created date is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime CreatedDate {get;set;}
        
        [Display(Name ="Last modified date" )]
        [Required(ErrorMessage = "Last modified date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}")]
        [DataType(DataType.DateTime)]
        public System.DateTime LastModifiedDate {get;set;}
        
        [Display(Name ="Created by" )]
        [Required(ErrorMessage = "Created by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String CreatedBy {get;set;}
        
        [Display(Name ="Last modified by" )]
        [Required(ErrorMessage = "Last modified by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String LastModifiedBy {get;set;}
  



        
        [Display(Name ="Grade" )]
        public object Grade {get;set;}
        
        [Display(Name ="Position description" )]
        public object PositionDescription {get;set;}
        
        [Display(Name ="Role description" )]
        public object RoleDescription {get;set;}
        
        [Display(Name ="Status value" )]
        public object StatusValue {get;set;}
        
        [Display(Name ="Role position description histories" )]
        public object RolePositionDescriptionHistories {get;set;}
        
        [Display(Name ="Trim record" )]
        public object TrimRecord {get;set;}
        
        [Display(Name ="Positions" )]
        public object Positions {get;set;}
  

    }
}


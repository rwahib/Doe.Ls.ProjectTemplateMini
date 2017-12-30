


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(SysUserMetadata))]
    public partial class SysUser
    {
        public override string ToString()
        {
            return string.Format("{0}", this.FirstName);
        }
    }

    public class SysUserMetadata
    {
        
        [Display(Name ="User id" )]
        [Required(ErrorMessage = "User id is required")]
        [MaxLength(64, ErrorMessage = "Exceeding the max length, allowed only 64 character")]
        public System.String UserId {get;set;}
        
        [Display(Name ="First name" )]
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(32, ErrorMessage = "Exceeding the max length, allowed only 32 character")]
        public System.String FirstName {get;set;}
        
        [Display(Name ="Last name" )]
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(32, ErrorMessage = "Exceeding the max length, allowed only 32 character")]
        public System.String LastName {get;set;}
        
        [Display(Name ="Email" )]
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(260, ErrorMessage = "Exceeding the max length, allowed only 260 character")]
        [DataType(DataType.EmailAddress)]
        public System.String Email {get;set;}
        
        [Display(Name ="Primary phone" )]
        [MaxLength(260, ErrorMessage = "Exceeding the max length, allowed only 260 character")]
        [DataType(DataType.PhoneNumber)]
        public System.String PrimaryPhone {get;set;}
        
        [Display(Name ="Note" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String Note {get;set;}
        
        [Display(Name ="Active" )]
        [Required(ErrorMessage = "Active is required")]
        public System.Boolean Active {get;set;}
        
        [Display(Name ="Created date" )]
        [Required(ErrorMessage = "Created date is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime CreatedDate {get;set;}
        
        [Display(Name ="Created by" )]
        [Required(ErrorMessage = "Created by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String CreatedBy {get;set;}
        
        [Display(Name ="Last modified date" )]
        [Required(ErrorMessage = "Last modified date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}")]
        [DataType(DataType.DateTime)]
        public System.DateTime LastModifiedDate {get;set;}
        
        [Display(Name ="Last modified by" )]
        [Required(ErrorMessage = "Last modified by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String LastModifiedBy {get;set;}
  



        
        [Display(Name ="Sys user roles" )]
        public object SysUserRoles {get;set;}
  

    }
}


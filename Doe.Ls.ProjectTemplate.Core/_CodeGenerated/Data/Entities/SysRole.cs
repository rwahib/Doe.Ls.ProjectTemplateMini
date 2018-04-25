 



using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(SysRoleMetadata))]
    public partial class SysRole
    {
        public override string ToString()
        {
            return string.Format("{0}", this.RoleApiName);
        }
    }

    public class SysRoleMetadata
    {
        
        [Display(Name ="Role id" )]
        [Required(ErrorMessage = "Role id is required")]
        public System.Int32 RoleId {get;set;}
        
        [Display(Name ="Role title" )]
        [Required(ErrorMessage = "Role title is required")]
        [MaxLength(128, ErrorMessage = "Exceeding the max length, allowed only 128 character")]
        public System.String RoleTitle {get;set;}
        
        [Display(Name ="Role api name" )]
        [MaxLength(128, ErrorMessage = "Exceeding the max length, allowed only 128 character")]
        public System.String RoleApiName {get;set;}
        
        [Display(Name ="Role description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String RoleDescription {get;set;}
        
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
  



        
        [Display(Name ="General logs" )]
        public object GeneralLogs {get;set;}
        
        [Display(Name ="Sys user roles" )]
        public object SysUserRoles {get;set;}
  

    }
}


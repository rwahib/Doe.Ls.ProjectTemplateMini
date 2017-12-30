


using System;
using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(SysUserRoleMetadata))]
    public partial class SysUserRole
    {
        public override string ToString()
        {
            return $"{this.SysUser.UserId}-{this.SysRole.RoleId}-{this.SysRole.RoleApiName}";
        }

        public bool IsActive()
            {
            return (DateTime.Now >= ActiveFrom) && (DateTime.Now <= (ActiveTo ?? DateTime.MaxValue));
            }
        }

    public class SysUserRoleMetadata
    {
        
        [Display(Name ="User id" )]
        [Required(ErrorMessage = "User id is required")]
        [MaxLength(64, ErrorMessage = "Exceeding the max length, allowed only 64 character")]
        public System.String UserId {get;set;}
        
        [Display(Name ="Role id" )]
        [Required(ErrorMessage = "Role id is required")]
        public System.Int32 RoleId {get;set;}
        
        [Display(Name ="Structure id" )]
        [Required(ErrorMessage = "Structure id is required")]
        public System.Int32 StructureId {get;set;}
        
        [Display(Name ="Org level id" )]
        [Required(ErrorMessage = "Org level id is required")]
        public System.Int32 OrgLevelId {get;set;}
        
        [Display(Name ="Active from" )]
        [Required(ErrorMessage = "Active from is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime ActiveFrom {get;set;}
        
        [Display(Name ="Active to" )]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime ActiveTo {get;set;}
        
        [Display(Name ="Note" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String Note {get;set;}
        
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
  



        
        [Display(Name ="Org level" )]
        public object OrgLevel {get;set;}
        
        [Display(Name ="Sys role" )]
        public object SysRole {get;set;}
        
        [Display(Name ="Sys user" )]
        public object SysUser {get;set;}
  

    }
}


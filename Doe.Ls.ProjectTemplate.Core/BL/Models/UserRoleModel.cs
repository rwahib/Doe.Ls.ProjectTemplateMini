using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
{
    [MetadataType(typeof(UserRoleModelMetadata))]
    public class UserRoleModel
    {
        public int RoleId { get; set; }
        public int OrgLevelId { get; set; }
        public string OrgLevelName { get; set; }
        public string StructureId { get; set; }
        public bool IsActive { get; set; }
        public string OrgObjetcName { get; set; }

        public DateTime ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }

        public string UserId { get; set; }
        public string DisplayedName { get; set; }        
        public string Email { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public override string ToString()
        {
            return $"{Email}-{RoleName}: {OrgLevelName}-{StructureId}-{OrgObjetcName}";
        }

        public Enums.UserRole UserRole => (Enums.UserRole) RoleId;

        public string RoleName
        {
            get { return UserRole.GetDescription(); }
        }

        public Enums.OrgLevel OrgLevel => (Enums.OrgLevel) OrgLevelId;

        #region PropertyNames
        public const string RoleIdProp = "RoleId";
        public const string RoleNameProp = "RoleName";
        public const string OrgLevelIdProp = "OrgLevelId";

        public const string OrgLevelNameProp = "OrgLevelName";
        public const string StructureIdProp = "StructureId";
        public const string IsActiveProp = "IsActive";

        public const string OrgObjetcNameProp = "OrgObjetcName";
        public const string ActiveFromProp = "ActiveFrom";
        public const string ActiveToProp = "ActiveTo";

        public const string LastModifiedDateProp = "LastModifiedDate";
        public const string LastModifiedByProp = "LastModifiedBy";
        public const string UserIdProp = "UserId";

        public const string DisplayedNameProp = "DisplayedName";
        public const string EmailProp = "Email";
        public const string NoteProp = "Note";

        public const string CreatedByProp = "CreatedBy";

        public static List<UiPropertyItem> GetAllUserRoleProperties()
        {
            var propList = new List<UiPropertyItem>
            {
                new UiPropertyItem(RoleIdProp),
                new UiPropertyItem(RoleNameProp),
                new UiPropertyItem(OrgLevelIdProp),

                new UiPropertyItem(OrgLevelNameProp),
                new UiPropertyItem(StructureIdProp),
                new UiPropertyItem(IsActiveProp),

                new UiPropertyItem(OrgObjetcNameProp),
                new UiPropertyItem(ActiveFromProp),
                new UiPropertyItem(ActiveToProp),

                new UiPropertyItem(LastModifiedDateProp),
                new UiPropertyItem(LastModifiedByProp),
                new UiPropertyItem(UserIdProp),

                new UiPropertyItem(DisplayedNameProp),
                new UiPropertyItem(EmailProp),
                new UiPropertyItem(NoteProp),

                new UiPropertyItem(CreatedByProp),
                
            };


            return propList;

        }

#endregion

        public void SetDefaults(ServiceRepository serviceRepository)
        {
            switch (UserRole)
            {
                case Enums.UserRole.SystemAdministrator:
                    StructureId = "-1";                    
                    OrgLevelId = (int)Enums.OrgLevel.Application;
                    break;
                case Enums.UserRole.Administrator:                    
                case Enums.UserRole.PowerUser:
                case Enums.UserRole.HRDataEntry:
                    
                    StructureId = "-1";
                    OrgLevelId = (int)Enums.OrgLevel.Application;
                    break;
                
                case Enums.UserRole.BusinessUnitDataEntry:
                case Enums.UserRole.BusinessUnitAuthor:
                    OrgLevelId = (int)Enums.OrgLevel.BusinessUnit;
                    break;
                case Enums.UserRole.DirectorateDataEntry:

                case Enums.UserRole.DirectorateEndorser:
                    OrgLevelId = (int)Enums.OrgLevel.Directorate;
                    break;

                case Enums.UserRole.DivisionEditor:
                    
                case Enums.UserRole.DivisionApprover:
                    OrgLevelId = (int)Enums.OrgLevel.Division;
                    break;
                }
            OrgLevelName = OrgLevel.GetDescription();
            if (OrgLevel != Enums.OrgLevel.Application && OrgLevel != Enums.OrgLevel.NA)
            {
                if (string.IsNullOrWhiteSpace(OrgObjetcName))
                {
                    if (this.OrgLevelId == (int) Enums.OrgLevel.Division)
                    {
                        var div = serviceRepository.ExecutiveRepository().GetExecutiveByCode(this.StructureId);
                        this.OrgObjetcName = div == null ? "" : div.ExecutiveTitle;
                    }
                    if (this.OrgLevelId == (int) Enums.OrgLevel.Directorate)

                    {
                        var dir =
                            serviceRepository.DirectorateRepository().GetDirectorateById(this.StructureId.ToInteger());
                        this.OrgObjetcName = dir == null ? "" : dir.DirectorateName;
                    }
                    if (this.OrgLevelId == (int) Enums.OrgLevel.BusinessUnit)
                    {
                        var bu = serviceRepository.BusinessUnitRepository().GetBUnitById(this.StructureId.ToInteger());
                        this.OrgObjetcName = bu == null ? "" : bu.BUnitName;
                    }
                }
            }
        }

        public void SetDefaults(SysUser user, ServiceRepository serviceRepository)
        {
            UserId = user.UserId;
            Email = user.Email;
            DisplayedName = user.DisplayedName();
            SetDefaults(serviceRepository);
            }

        }

    public class UserRoleModelMetadata
    {
        public int RoleId { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        public int OrgLevelId { get; set; }

        [Display(Name = "Org level")]
        public string OrgLevelName { get; set; }

        public string StructureId { get; set; }

        [UIHint("AccessibleBooleanTrueFalse")]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Orgnisational name")]
        public string OrgObjetcName { get; set; }

        [Display(Name = "Active from")]
        public DateTime ActiveFrom { get; set; }

        [Display(Name = "Active to")]
        public DateTime? ActiveTo { get; set; }

        [Display(Name = "Last modified date")]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "Last modified by")]
        public string LastModifiedBy { get; set; }

        [Display(Name = "User id")]
        public string UserId { get; set; }

        [Display(Name = "Displayed name")]
        public string DisplayedName { get; set; }

        public string Email { get; set; }

        public override string ToString()
        {
            return $"{Email}-{RoleName}: {OrgLevelName}-{StructureId}-{OrgObjetcName}";
        }
    }

}
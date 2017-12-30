using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using Doe.Ls.EntityBase;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions
    {
    /// <summary>
    /// This class for all user information conversion
    /// </summary>
    [MetadataType(typeof(UserInfoExtensionMetadata))]
    public class UserInfoExtension : UserInfo
        {
        private List<UserRoleModel> _activeRoleOrgLevelList;
        public List<UserRoleModel> ActiveRoleOrgLevelList
            {
            get
                {
                return _activeRoleOrgLevelList ?? (_activeRoleOrgLevelList = new List<UserRoleModel>());
                }
            set { _activeRoleOrgLevelList = value; }
            }
        public bool IsRoleInitialised { get; set; }

        public DefaultOrganisationalModel DefaultOrganisationalModel { get; private set; }

        #region Role properties and methods

        public bool IsSystemAdmin
            {
            get { return ActiveRoleOrgLevelList?.Any(r => r.RoleId == (int)Enums.UserRole.SystemAdministrator) ?? false; }
            }
        public bool IsAdministrator
            {
            get { return ActiveRoleOrgLevelList?.Any(r => r.RoleId == (int)Enums.UserRole.Administrator) ?? false; }
            }

        public bool IsPowerUser
            {
            get { return ActiveRoleOrgLevelList?.Any(r => r.RoleId == (int)Enums.UserRole.PowerUser) ?? false; }
            }

        public bool IsDivisionApprover
            {
            get { return ActiveRoleOrgLevelList?.Any(r => r.RoleId == (int)Enums.UserRole.DivisionApprover) ?? false; }
            }

        public bool IsDivisionEditor
            {
            get { return ActiveRoleOrgLevelList?.Any(r => r.RoleId == (int)Enums.UserRole.DivisionEditor) ?? false; }
            }

        public bool IsDirectorateDataEntry
            {
            get { return ActiveRoleOrgLevelList?.Any(r => r.RoleId == (int)Enums.UserRole.DirectorateDataEntry) ?? false; }
            }

        public bool IsDirectorateEndorser
            {
            get { return ActiveRoleOrgLevelList?.Any(r => r.RoleId == (int)Enums.UserRole.DirectorateEndorser) ?? false; }
            }

        public bool IsBusinessUnitAuthor
            {
            get { return ActiveRoleOrgLevelList?.Any(r => r.RoleId == (int)Enums.UserRole.BusinessUnitAuthor) ?? false; }
            }

        public bool IsBusinessUnitDataEntry
            {
            get { return ActiveRoleOrgLevelList?.Any(r => r.RoleId == (int)Enums.UserRole.BusinessUnitDataEntry) ?? false; }
            }

        public bool IsDoEUser
            {
            get
                {
                return !ActiveRoleOrgLevelList.Any() ||
                       ActiveRoleOrgLevelList.All(r => r.RoleId == Enums.UserRole.DoEUser.ToInteger());
                }
            }

        public bool IsGuest
            {
            get
                {
                if(!ActiveRoleOrgLevelList.Any()) return true;
                return ActiveRoleOrgLevelList?.Any(r => r.RoleId == (int)Enums.UserRole.Guest) ?? false;
                }
            }

        public bool HasAnyAdminRole()
            {
            return IsSystemAdmin || IsAdministrator || IsPowerUser
                || IsBusinessUnitAuthor || IsBusinessUnitDataEntry
                || IsDirectorateDataEntry || IsDirectorateEndorser
                || IsDivisionApprover || IsDivisionEditor || IsHrDataEntry;
            }

        public bool HasAnyAdminRoleExceptHr()
        {
            return IsSystemAdmin || IsAdministrator || IsPowerUser
                || IsBusinessUnitAuthor || IsBusinessUnitDataEntry
                || IsDirectorateDataEntry || IsDirectorateEndorser
                || IsDivisionApprover || IsDivisionEditor;
        }

        public bool HasAdminOrPowerRole()
            {
            return IsSystemAdmin || IsAdministrator || IsPowerUser;
            }

        public bool HasDoERoleOnly()
            {
            return !HasAnyAdminRole() && IsDoEUser;
            }
        public bool HasApprovalRole()
            {
            return IsBusinessUnitAuthor || IsDirectorateEndorser || IsDivisionApprover;
            }
        public bool IsHrDataEntry
            {
            get { return ActiveRoleOrgLevelList?.Any(r => r.RoleId == (int)Enums.UserRole.HRDataEntry) ?? false; }

            }
        #endregion

        #region Static properties and methods
        public static UserInfoExtension GuestUser => new UserInfoExtension
            {
            UserName = Cnt.Guest,
            FirstName = Cnt.Guest,
            SurName = string.Empty,
            IsRoleInitialised = true,
            Email = $"{Cnt.Guest}@guest.com",
            DisplayName = $"{Cnt.Guest} user",

            };

        public static UserInfoExtension SystemUser => new UserInfoExtension
            {
            UserName = Enums.Cnt.System,
            FirstName = Enums.Cnt.System,
            SurName = string.Empty,
            IsRoleInitialised = true,
            Email = $"{Enums.Cnt.System}@admin.com",
            DisplayName = $"{Enums.Cnt.System} user",

            };

        public static UserInfoExtension MapUserInfo(UserInfo user)
            {
            return new UserInfoExtension
                {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Title = user.Title,
                UserName = user.UserName,
                SurName = user.SurName,
                FirstName = user.FirstName,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.DepartmentName
                };
            }

        public static UserInfoExtension MapSysUser(SysUser sysUser, IRepositoryFactory factory)
            {
            var serviceRepository = new ServiceRepository(factory);
            var uExt = new UserInfoExtension
                {

                UserName = sysUser.UserId,
                FirstName = sysUser.FirstName,
                Email = sysUser.Email,
                SurName = sysUser.LastName,
                Roles = sysUser.SysUserRoles.Select(ur => ur.SysRole.RoleApiName).ToArray(),
                };

            uExt.InitialiseRoles(factory);
            
            return uExt;
            }

        #endregion

        public void InitialiseRoles(IRepositoryFactory factory)
        {
           
            if(!this.IsRoleInitialised)
                {
                var service = new ServiceRepository(factory);
                var sysUser = service.SysUserRepository().GetSysUserByUserName(this.UserName);

                this.ActiveRoleOrgLevelList = new List<UserRoleModel>();


                if(sysUser != null)
                    {                   
                    foreach(
                        var sr in
                            sysUser.SysUserRoles.Where(ar => ar.IsActive()).ToArray())
                        {
                        var roleOrgLevel = sr.ToUserOrgLevelObject(service);
                        this.ActiveRoleOrgLevelList.Add(roleOrgLevel);
                        }

                    if(
                        this.ActiveRoleOrgLevelList.ToList().All(ur => ur.RoleId != Enums.UserRole.DoEUser.ToInteger()))
                        {
                        this.ActiveRoleOrgLevelList.Add(new UserRoleModel
                            {
                            RoleId = Enums.UserRole.DoEUser.ToInteger(),
                            OrgLevelId = Enums.OrgLevel.NA.ToInteger(),
                            OrgLevelName = Enums.OrgLevel.NA.ToString(),
                            StructureId = "-1",
                            IsActive = true
                            });
                        }

                    }
                    DefaultOrganisationalModel = DefaultOrganisationalModel.BuildDefaultOrganisationalModel(this,
                        factory);
                this.IsRoleInitialised = true;
                }
            }

        public SysUser ToSysUser()
            {
            return new SysUser
                {
                UserId = UserName,
                Email = Email,
                LastName = SurName,
                FirstName = FirstName,
                };
            }

        public string DisplayRoles()
            {
            var sb = new StringBuilder();
            foreach(var role in ActiveRoleOrgLevelList)
                {
                var structureList = new int[]
                {
                    Enums.OrgLevel.Division.ToInteger(), Enums.OrgLevel.Directorate.ToInteger(),
                    Enums.OrgLevel.BusinessUnit.ToInteger(),
                    Enums.OrgLevel.Division.ToInteger()
                };
                if(structureList.Contains(role.OrgLevelId))
                    {
                    sb.AppendLine($"{role.RoleName.Wordify()} for ({role.OrgLevelName}: {role.StructureId}-{role.OrgObjetcName}), ");
                    }
                else
                    {
                    sb.AppendLine($"{role.RoleName.Wordify()}, ");
                    }


                }
            return sb.ToString().TrimEnd(new[] { ',', ' ', '\n', '\r' });
            }

        public override string CurrentRole
            {
            get
                {
                if(!IsRoleInitialised || UserName == Cnt.Guest) return Enums.UserRoleValues.Guest;

                if(IsSystemAdmin) return Enums.UserRoleValues.SystemAdministrator;
                if(IsAdministrator) return Enums.UserRoleValues.Administrator;
                if(IsPowerUser) return Enums.UserRoleValues.PowerUser;

                if(IsDivisionApprover) return Enums.UserRoleValues.DivisionApprover;
                if(IsDivisionEditor) return Enums.UserRoleValues.DivisionEditor;

                if(IsDirectorateEndorser) return Enums.UserRoleValues.DirectorateEndorser;
                if(IsDirectorateDataEntry) return Enums.UserRoleValues.DirectorateDataEntry;

                if(IsBusinessUnitAuthor) return Enums.UserRoleValues.BusinessUnitAuthor;
                if(IsBusinessUnitDataEntry) return Enums.UserRoleValues.BusinessUnitDataEntry;
                if(IsHrDataEntry) return Enums.UserRoleValues.HRDataEntry;

                if(IsDoEUser) return Enums.UserRoleValues.DoEUser;

                return Enums.UserRoleValues.Guest;
                }
            set { throw new ReadOnlyException($"{nameof(CurrentRole)} is read only"); }
            }

        public Enums.UserRole CurrentRoleEnum
            {
            get
                {

                var result = EnumExtension.Parse<Enums.UserRole>(CurrentRole);
                return result;


                }
            }
        }

    public class UserInfoExtensionMetadata
        {
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        public string SurName { get; set; }

        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
        /// <remarks/>


        /// <remarks/>
        [Display(Name = "School id")]

        public int SchoolId
            { get; set; }

        /// <remarks/>
        [Display(Name = "School name")]
        public string SchoolName
            {
            get;
            set;
            }

        [Display(Name = "Department id")]
        public string DepartmentId
            {
            get;
            set;
            }

        [Display(Name = "Department name")]
        public string DepartmentName
            {
            get;
            set;
            }
        public string[] Groups
            {
            get;
            set;
            }

        /// <remarks/>
        [Display(Name = "Member of")]
        public string[] MemberOf
            {
            get;
            set;
            }

        public string[] Roles
            {
            get;
            set;
            }

        /// <remarks/>
        public string Path
            {
            get;
            set;
            }

        public string Phone { get; set; }

        [Display(Name = "Active roles")]
        public object ActiveRoleOrgLevelList { get; set; }

        [Display(Name = "role initialised")]
        public bool IsRoleInitialised { get; set; }
        [Display(Name = "System admin")]
        public bool IsSystemAdmin { get; set; }
        [Display(Name = "Administrator")]
        public bool IsAdministrator { get; set; }
        [Display(Name = "Power user")]
        public bool IsPowerUser { get; set; }
        [Display(Name = "Division approver")]
        public bool IsDivisionApprover { get; set; }
        [Display(Name = "Division editor")]
        public bool IsDivisionEditor { get; set; }
        [Display(Name = "Is directorate dataEntry")]
        public bool IsDirectorateDataEntry { get; set; }
        [Display(Name = "Is directorate endorser")]
        public bool IsDirectorateEndorser
            { get; set; }

        [Display(Name = "Business unit author")]
        public bool IsBusinessUnitAuthor
            { get; set; }

        [Display(Name = "Business unit data entry")]
        public bool IsBusinessUnitDataEntry
            { get; set; }

        [Display(Name = "DoE user")]
        public bool IsDoEUser
            { get; set; }

        [Display(Name = "Guest")]
        public bool IsGuest
            { get; set; }

        [Display(Name = "HR data entry")]
        public bool IsHrDataEntry
            { get; set; }

        [Display(Name = "Current role")]
        public string CurrentRole
            { get; set; }


        [Display(Name = "Current role")]
        public Enums.UserRole CurrentRoleEnum
            { get; set; }

        }
    }

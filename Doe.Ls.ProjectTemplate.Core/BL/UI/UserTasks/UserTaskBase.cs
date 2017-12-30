
using System;
using System.Collections.Generic;
using System.Linq;
using Doe.Ls.EntityBase;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.Dashboards;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks
    {

    public abstract class UserTaskBase : IUserTask
        {
        private ServiceRepository _serviceRepository;
        private Dashboard _dashboard;
        public IRepositoryFactory RepositoryFactory { get; set; }
        public UserInfoExtension User { get; }

        public string Name { get; }
        public string Description { get; }
        public string DashboardTitle { get; }
        public virtual Enums.Privilege GetSysUserRolePrivilege(string userId, int roleId, int orgLevelId, int structureId)
            {
            return new Enums.Privilege
                {
                CanRead = true
                };
            }
        public Dashboard Dashboard => _dashboard ?? (_dashboard = BuildDashboard(ServiceRepository));
        public virtual Enums.Privilege GetSysUserPrivilege(string userId)
            {
            return new Enums.Privilege
                {
                CanRead = true
                };
            }

        protected virtual Dashboard BuildDashboard(ServiceRepository serviceRepository)
            {

            var dashboard = new Dashboard
                {

                DashboardTitle = $"{this.User.CurrentRole.Wordify()} dashboard",
                DashboardSections = new List<DashboardSection>()
                {

                DashboardSection.SearchAndBrowseTasks(this),
                //DashboardSection.WorkflowTasks(this),
                DashboardSection.OrganisationalStructureTasks(this),
                //DashboardSection.PositionsTasks(this),
                //DashboardSection.RoleDescriptionsTasks(this),
                //DashboardSection.PositionDescriptionsTasks(this),
                DashboardSection.UserRolesAndSecurityTasks(this),
                DashboardSection.ApplicationConfigurationTasks(this),
                DashboardSection.SystemTasks(this),


                }
                };


            return dashboard;
            }

        public virtual bool CanApproveDocuments()
            {
            if(User.IsSystemAdmin || User.IsDirectorateEndorser || User.IsDivisionApprover)
                {

                return true;
                }

            return false;

            }

        public virtual bool CanAccessOrganisationalStructure()
            {
            if(User.CurrentRole == Enums.UserRoleValues.DoEUser) return false;

            return true;

            }

        public bool CanAccessApplicationConfiguration()
            {
            return User.IsSystemAdmin || User.IsAdministrator || User.IsPowerUser;
            }

        public bool CanAccessRoleDescriptionTasks()
            {
            return User.IsSystemAdmin || User.IsAdministrator || User.IsPowerUser;
            }

        public bool CanAccessPositionDescriptionTasks()
            {
            return User.IsSystemAdmin || User.IsAdministrator || User.IsPowerUser;
            }

        public bool CanAccessUserRoleAndSecurityTasks()
            {
            return User.IsSystemAdmin || User.IsAdministrator || User.IsPowerUser;
            }

        public RolePosPropertyStateModel GetRolePosModelState(int id)
            {
            if(id == 0)
                {
                return RolePosPropertyStateModel.AllEnabled;
                }
            var rpd = ServiceRepository.RolePositionDescriptionRepository().GetRolePositionDescById(id);
            var isDraft = rpd.StatusValue.GetEnum() == Enums.StatusValue.Draft;

            if(!isDraft) return RolePosPropertyStateModel.AllDisabled;

            var hasPositionsLinked =
                ServiceRepository.PositionRepository().GetAllLinkedPositionsById(id).HasNonDeletedAny();
            if(hasPositionsLinked) return RolePosPropertyStateModel.AllDisabled;

            if(rpd.IsPositionDescription)
                {
                var hasFocausCriteria = ServiceRepository.PositionFocusCriteriaRepository()
                    .List()
                    .Any(l => l.PositionDescriptionId == id);
                if(hasFocausCriteria)
                    return new RolePosPropertyStateModel
                        {
                        GradeEnabled = false,
                        TitleEnabled = true,
                        DocumentNumberEnabled = true
                        };
                }
            else
                {
                var hasCapabilities = ServiceRepository.RoleCapabilityRepository()
             .List()
             .Any(c => c.RoleDescriptionId == id);
                if(hasCapabilities) return new RolePosPropertyStateModel
                    {
                    GradeEnabled = false,
                    TitleEnabled = true,
                    DocumentNumberEnabled = true
                    };
                }


            return RolePosPropertyStateModel.AllEnabled;
            }

        public bool CanEditPositionDescription(int roleDescriptionId)
            {
            /*if (User.IsDoEUser | User.IsGuest)
                return false;*/
            var canEdit = ServiceRepository.PositionRepository()
                .List()
                .Any(l => l.RolePositionDescriptionId == roleDescriptionId);
            return !canEdit;
            }


        public bool CanDownloadRoleDesc(int roleDescriptionId)
            {
            var rolePositionDesc = ServiceRepository.RolePositionDescriptionRepository()
               .List()
               .FirstOrDefault(l => l.RolePositionDescId == roleDescriptionId);

            if(rolePositionDesc != null && rolePositionDesc.StatusId != (int)Enums.StatusValue.Draft)
                return true;

            return false;
            }

        public bool HasLinkedPositions(int rolePositionDescId)
            {
            var hasPositions = ServiceRepository.PositionRepository()
               .List()
               .Any(l => l.RolePositionDescriptionId == rolePositionDescId);

            return hasPositions;
            }

        public bool CanEditNotes(PositionInformation positionInfo, PositionNote currentNote)
            {
            if(positionInfo == null || positionInfo.PositionNotes == null)
                {
                return false;
                }
            var latestNoteId = positionInfo.PositionNotes.Any() ? positionInfo.PositionNotes.OrderByDescending(p => p.LastModifiedDate).First().PositionNoteId : 0;
            if(User.UserName == currentNote.CreatedBy && currentNote.PositionNoteId.ToString() == latestNoteId.ToString())
                return true;
            return false;
            }

        public bool CanViewRolePositiondesc()
            {
            if(User.IsGuest)
                return false;
            if(User.IsPowerUser || User.IsAdministrator || User.IsBusinessUnitAuthor || User.IsBusinessUnitDataEntry || User.IsDirectorateDataEntry
                || User.IsDirectorateEndorser || User.IsDivisionApprover || User.IsDivisionEditor || User.IsSystemAdmin)
                return true;
            return false;
            }

        public bool CanEditRoleDesc(int roleDescriptionId)
            {
            var canEdit = ServiceRepository.RolePositionDescriptionRepository()
                .List()
                .Any(l => l.RolePositionDescId == roleDescriptionId &&
                (l.StatusId == (int)Enums.StatusValue.Draft || l.StatusId == (int)Enums.StatusValue.Imported));
            return canEdit;
            }

        public Enums.Privilege GetWorkflowObjectPrivilege(WorkflowObjectType workflowObjectType)
            {
            var priv = Enums.Privilege.AccessDeniedPrivilege;
            if(User.HasAdminOrPowerRole() || User.IsBusinessUnitDataEntry || User.IsBusinessUnitAuthor ||
                User.IsDirectorateEndorser ||
                User.IsDirectorateDataEntry || User.IsDivisionApprover || User.IsDivisionEditor)
                {
                priv.CanCreate = true; // then we need to filter down for each organisation ( only for positions)
                priv.CanEdit = true; // then we need to filter down for specific position

                }

            return priv;
            }

        #region Executive security
        public Enums.Privilege GetExecutivePrivilege()
            {
            if(User.HasAdminOrPowerRole())
                {
                return new Enums.Privilege
                    {
                    FullControl = true,

                    };
                }
            else
            if(User.HasAnyAdminRole())
                {
                return new Enums.Privilege
                    {
                    CanRead = true,

                    };
                }
            return new Enums.Privilege
                {


                };
            }
        public Enums.Privilege GetExecutivePrivilege(Executive executive)
            {
            var priv = new Enums.Privilege();
            if(executive == null)
                {
                return priv;
                }
            if(executive.ExecutiveCod == Enums.Cnt.Na.ToString())
                {
                priv.SetAll(false);
                if(User.IsSystemAdmin)
                    {
                    priv.CanEdit = true;
                    priv.CanRead = true;

                    }
                return priv;
                }
            if(User.HasAdminOrPowerRole())
                {
                priv.SetAll(false);
                priv.CanRead = true;

                priv.CanEdit = true;

                var rep = ServiceRepository.ExecutiveRepository();
                rep.LoadNavigationProperty(executive, e => e.Directorates);

                if(!executive.Directorates.Any())
                    {
                    priv.CanDelete = true;
                    }
                return priv;
                }
            priv.SetAll(false);
            return priv;

            }
        public Enums.Privilege GetExecutivePrivilege(string executiveCode)
            {
            var executive = ServiceRepository.ExecutiveRepository().GetExecutiveByCode(executiveCode);
            return GetExecutivePrivilege(executive);
            }

        #endregion

        #region Directorate security
        public Enums.Privilege GetDirectoratePrivilege()
            {
            if(User.HasAdminOrPowerRole())
                {
                return new Enums.Privilege
                    {
                    FullControl = true,

                    };
                }
            else
            if(User.HasAnyAdminRole())
                {
                return new Enums.Privilege
                    {
                    CanRead = true,

                    };
                }
            return new Enums.Privilege
                {


                };
            }
        public Enums.Privilege GetDirectoratePrivilege(Directorate directorate)
            {
            var priv = new Enums.Privilege();
            if(directorate == null)
                {
                return priv;
                }
            if(directorate.DirectorateId == Enums.Cnt.Na)
                {
                priv.SetAll(false);
                if(User.IsSystemAdmin)
                    {
                    priv.CanEdit = true;
                    priv.CanRead = true;

                    }
                return priv;
                }
            if(User.HasAdminOrPowerRole())
                {
                priv.SetAll(false);
                priv.CanRead = true;

                priv.CanEdit = true;

                var rep = ServiceRepository.DirectorateRepository();
                rep.LoadNavigationProperty(directorate, d => d.BusinessUnits);
                rep.LoadNavigationProperty(directorate, d => d.FunctionalAreas);

                if(!(directorate.BusinessUnits.Any() || directorate.FunctionalAreas.Any()))
                    {
                    priv.CanDelete = true;
                    }
                return priv;
                }
            priv.SetAll(false);

            return priv;

            }
        public Enums.Privilege GetDirectoratePrivilege(int directorateId)
            {
            var directorate = ServiceRepository.DirectorateRepository().GetDirectorateById(directorateId);
            return GetDirectoratePrivilege(directorate);
            }

        #endregion

        #region Business unit security

        public Enums.Privilege GetBUnitPrivilege()
            {
            if(User.HasAdminOrPowerRole())
                {
                return new Enums.Privilege
                    {
                    FullControl = true,

                    };
                }
            else
            if(User.HasAnyAdminRole())
                {
                return new Enums.Privilege
                    {
                    CanRead = true,

                    };
                }
            return new Enums.Privilege
                {


                };
            }
        public Enums.Privilege GetBUnitPrivilege(BusinessUnit bUnit)
            {
            var priv = new Enums.Privilege();
            if(bUnit == null)
                {
                return priv;
                }
            if(bUnit.BUnitId == Enums.Cnt.Na)
                {
                priv.SetAll(false);
                if(User.IsSystemAdmin)
                    {
                    priv.CanEdit = true;
                    priv.CanRead = true;

                    }
                return priv;
                }
            if(User.HasAdminOrPowerRole())
                {
                priv.SetAll(false);
                priv.CanRead = true;

                priv.CanEdit = true;
                ServiceRepository.BusinessUnitRepository().LoadNavigationProperty(bUnit, u => u.Units);
                if(!bUnit.Units.Any())
                    {
                    priv.CanDelete = true;
                    }
                return priv;
                }
            priv.SetAll(false);

            return priv;

            }
        public Enums.Privilege GetBUnitPrivilege(int bUnitId)
            {
            var bUnit = ServiceRepository.BusinessUnitRepository().GetBUnitById(bUnitId);
            return GetBUnitPrivilege(bUnit);
            }

        #endregion

        #region Function area security

        public Enums.Privilege GetFunctionAreaPrivilege()
            {
            if(User.HasAdminOrPowerRole())
                {
                return new Enums.Privilege
                    {
                    FullControl = true,

                    };
                }
            else
            if(User.HasAnyAdminRole())
                {
                return new Enums.Privilege
                    {
                    CanRead = true,

                    };
                }
            return new Enums.Privilege
                {


                };
            }
        public Enums.Privilege GetFunctionAreaPrivilege(FunctionalArea functionalArea)
            {
            var priv = new Enums.Privilege();
            if(functionalArea == null)
                {
                return priv;
                }
            if(functionalArea.FuncationalAreaId == Enums.Cnt.Na)
                {
                priv.SetAll(false);
                if(User.IsSystemAdmin)
                    {
                    priv.CanEdit = true;
                    priv.CanRead = true;

                    }
                return priv;
                }
            if(User.HasAdminOrPowerRole())
                {
                priv.SetAll(false);
                priv.CanRead = true;

                priv.CanEdit = true;
                var rep = ServiceRepository.FunctionalAreaRepository();
                rep.LoadNavigationProperty(functionalArea, f => f.Units);


                if(!functionalArea.Units.Any())
                    {
                    priv.CanDelete = true;
                    }
                return priv;
                }
            priv.SetAll(false);

            return priv;

            }
        public Enums.Privilege GetFunctionAreaPrivilege(int funcationalAreaId)
            {
            var functionArea = ServiceRepository.FunctionalAreaRepository().GetFunctionalAreaById(funcationalAreaId);
            return GetFunctionAreaPrivilege(functionArea);
            }


        #endregion

        #region Team security

        public Enums.Privilege GetTeamPrivilege()
            {
            if(User.HasAdminOrPowerRole())
                {
                return Enums.Privilege.FullContolPrivilege;

                }
            else if(User.HasAnyAdminRole())
                {
                return Enums.Privilege.ReadOnlyPrivilege;
                }
            return Enums.Privilege.AccessDeniedPrivilege;
            }

        public Enums.Privilege GetTeamPrivilege(Unit unit)
            {
            if(unit == null)
                {
                return Enums.Privilege.AccessDeniedPrivilege;
                }
            if(unit.UnitId == Enums.Cnt.Na)
                {

                if(User.IsSystemAdmin)
                    {
                    return Enums.Privilege.ModifyPrivilege;

                    }
                return Enums.Privilege.AccessDeniedPrivilege;
                }
            if(User.HasAdminOrPowerRole())
                {

                if(!unit.Positions.Any())
                    {
                    return Enums.Privilege.FullContolPrivilege;
                    }
                return Enums.Privilege.ModifyPrivilege;
                }

            else if(User.HasAnyAdminRole())
                {
                return Enums.Privilege.ReadOnlyPrivilege;
                }
            return Enums.Privilege.AccessDeniedPrivilege;
            }

        public Enums.Privilege GetTeamPrivilege(int unitId)
            {
            var unit = ServiceRepository.UnitRepository().GetUnitById(unitId);
            return GetTeamPrivilege(unit);
            }
        #endregion

        #region Position level security

        public Enums.Privilege GetPositionLevelPrivilege()
            {
            if(User.HasAdminOrPowerRole())
                {
                return Enums.Privilege.FullContolPrivilege;

                }
            else if(User.HasAnyAdminRole())
                {
                return Enums.Privilege.ReadOnlyPrivilege;
                }
            return Enums.Privilege.AccessDeniedPrivilege;
            }

        public Enums.Privilege GetPositionLevelPrivilege(PositionLevel positionLevel)
            {
            if(positionLevel == null)
                {
                return Enums.Privilege.AccessDeniedPrivilege;
                }
            if(positionLevel.PositionLevelId == Enums.Cnt.Na)
                {

                if(User.IsSystemAdmin)
                    {
                    return Enums.Privilege.ModifyPrivilege;

                    }
                return Enums.Privilege.AccessDeniedPrivilege;
                }
            if(User.HasAdminOrPowerRole())
                {
                ServiceRepository.PositionLevelRepository().LoadNavigationProperty(positionLevel, pl => pl.Positions);
                if(!positionLevel.Positions.Any())
                    {
                    return Enums.Privilege.FullContolPrivilege;
                    }
                return Enums.Privilege.ModifyPrivilege;
                }

            else if(User.HasAnyAdminRole())
                {
                return Enums.Privilege.ReadOnlyPrivilege;
                }
            return Enums.Privilege.AccessDeniedPrivilege;
            }

        public Enums.Privilege GetPositionLevelPrivilege(int positionLevelid)
            {
            var posLevel = ServiceRepository.PositionLevelRepository().GetPositionLevelById(positionLevelid);
            return GetPositionLevelPrivilege(posLevel);
            }
        #endregion

        #region grade security
        public Enums.Privilege GetGradePrivilege()
            {
            if(User.HasAdminOrPowerRole())
                {
                return Enums.Privilege.FullContolPrivilege;

                }
            else if(User.HasAnyAdminRole())
                {
                return Enums.Privilege.ReadOnlyPrivilege;
                }
            return Enums.Privilege.AccessDeniedPrivilege;
            }
        public Enums.Privilege GetGradePrivilege(Grade grade)
            {
            if(grade == null)
                {
                return Enums.Privilege.AccessDeniedPrivilege;
                }
            if(grade.GradeCode == Enums.Grade.NA)
                {

                if(User.IsSystemAdmin)
                    {
                    return Enums.Privilege.ModifyPrivilege;

                    }
                return Enums.Privilege.AccessDeniedPrivilege;
                }
            if(User.HasAdminOrPowerRole())
                {
                var rep = ServiceRepository.GradeRepository();
                rep.LoadNavigationProperty(grade, e => e.RolePositionDescriptions);

                if(!grade.RolePositionDescriptions.Any())
                    {
                    return Enums.Privilege.FullContolPrivilege;
                    }
                return Enums.Privilege.ModifyPrivilege;
                }

            else if(User.HasAnyAdminRole())
                {
                return Enums.Privilege.ReadOnlyPrivilege;
                }
            return Enums.Privilege.AccessDeniedPrivilege;
            }
        public Enums.Privilege GetGradePrivilege(string gradeCode)
            {
            var grade = ServiceRepository.GradeRepository().GetGradeByCode(gradeCode);
            return GetGradePrivilege(grade);
            }
        #endregion

        #region Location security
        public Enums.Privilege GetLocationPrivilege()
            {

            if(User.HasAdminOrPowerRole())
                {
                return Enums.Privilege.FullContolPrivilege;

                }
            else if(User.HasAnyAdminRole())
                {
                return Enums.Privilege.ReadOnlyPrivilege;
                }
            return Enums.Privilege.AccessDeniedPrivilege;
            }

        public Enums.Privilege GetLocationPrivilege(Location location)
            {
            if(location == null)
                {
                return Enums.Privilege.AccessDeniedPrivilege;
                }

            if(location.LocationId == Enums.Cnt.Na)
                {

                if(User.IsSystemAdmin)
                    {
                    return Enums.Privilege.ModifyPrivilege;

                    }
                return Enums.Privilege.AccessDeniedPrivilege;
                }
            if(User.HasAdminOrPowerRole())
                {
                ServiceRepository.LocationRepository().LoadNavigationProperty(location, l => l.Positions);
                if(!location.Positions.Any())
                    {
                    return Enums.Privilege.FullContolPrivilege;
                    }
                return Enums.Privilege.ModifyPrivilege;
                }

            else if(User.HasAnyAdminRole())
                {
                return Enums.Privilege.ReadOnlyPrivilege;
                }
            return Enums.Privilege.AccessDeniedPrivilege;
            }

        public Enums.Privilege GetLocationPrivilege(int locationid)
            {
            var location = ServiceRepository.LocationRepository().GetLocationByIdWithPositions(locationid);
            return GetLocationPrivilege(location);
            }

        #endregion

        #region Position security
        public Enums.Privilege GetPositionPrivilege()
            {
            if(User.HasAdminOrPowerRole())
                {
                return new Enums.Privilege
                    {
                    FullControl = true,

                    };
                }
            else
            if(User.HasAnyAdminRoleExceptHr())
                {
                return new Enums.Privilege
                    {
                    CanRead = true,
                    CanEdit = true,
                    CanCreate = true,
                    CanPerformActions = true
                    };
                }
            return new Enums.Privilege
                {
                CanRead = true

                };
            }

        public Enums.Privilege GetPositionDescriptionPrivilege()
            {
            var priv = Enums.Privilege.ReadOnlyPrivilege;
            if(!this.User.HasAnyAdminRoleExceptHr()) return priv;
            priv.CanCreate = true;
            priv.CanEdit = true;
            priv.CanPerformActions = true;
            return priv;
            }
        public Enums.Privilege GetRoleDescriptionPrivilege()
            {
            var priv = Enums.Privilege.ReadOnlyPrivilege;
            if(!this.User.HasAnyAdminRoleExceptHr()) return priv;
            priv.CanCreate = true;
            priv.CanEdit = true;
            priv.CanPerformActions = true;


            return priv;
            }
       
        public virtual IQueryable<Position> FilterPositions(IQueryable<Position> positions)
            {
            var validStatesIds = GetPositionStatus().Select(s => s.ToInteger()).ToArray();
            var filtered = positions.Where(l => l.PositionId != Enums.Cnt.Na && validStatesIds.Contains(l.StatusId));
            return filtered;
            }
        public abstract IQueryable<Position> GetApprovalPositions();


        public virtual List<Enums.StatusValue> GetPositionStatus()
            {
            var posStatusList = new List<Enums.StatusValue>
                {
                    Enums.StatusValue.Approved,
                    Enums.StatusValue.Imported,
                    Enums.StatusValue.Draft,
                    Enums.StatusValue.Submitted,
                    Enums.StatusValue.Endorsed,
                    Enums.StatusValue.Archived,
                    Enums.StatusValue.Deleted,

                };

            if(User.IsSystemAdmin)
                {
                return posStatusList;
                }
            else if(User.IsAdministrator || User.IsPowerUser)
                {
                var adminList = new List<Enums.StatusValue>(posStatusList);
                adminList.Remove(Enums.StatusValue.Deleted);
                return adminList;

                }
            else if(User.HasAnyAdminRole())
                {
                var approverlist = new List<Enums.StatusValue>(posStatusList);
                approverlist.Remove(Enums.StatusValue.Deleted);
                return approverlist;

                }
            else if(User.IsDoEUser)
                {
                return new List<Enums.StatusValue>
                {
                    Enums.StatusValue.Approved,
                    Enums.StatusValue.Imported,
                };
                }

            var msg = MessageHelper.AccessDeniedMessage();
            throw new InvalidOperationException(msg);
            }

        public virtual IQueryable<Executive> GetDivisionList()
            {

            return ServiceRepository.ExecutiveRepository().AllList();
            }

        public IQueryable<PositionDescription> GetPositionDescriptions()
            {
            var list = ServiceRepository.PositionDescriptionRepository().List();
            var rep = ServiceRepository.PositionDescriptionRepository();
            if(User.IsSystemAdmin)
                {
                return rep.List();
                }

            if(User.HasAdminOrPowerRole())
                {
                return rep.ActiveList();
                }
            if(User.HasAnyAdminRole())
                {
                return rep.ActiveList();
                }

            return rep.LiveList();

            }


        public IQueryable<RoleDescription> GetRoleDescriptions()
            {
            var rep = ServiceRepository.RoleDescriptionRepository();
            if(User.IsSystemAdmin)
                {
                return rep.List();
                }

            if(User.HasAdminOrPowerRole())
                {
                return rep.ActiveList();
                }
            if(User.HasAnyAdminRole())
                {
                return rep.ActiveList();
                }

            return rep.LiveList();

            }

        public bool CanViewStatus()
            {
            if(User.IsAdministrator || User.IsBusinessUnitAuthor || User.IsBusinessUnitDataEntry ||
                User.IsDirectorateDataEntry
                || User.IsDirectorateEndorser || User.IsDivisionApprover || User.IsDivisionEditor || User.IsPowerUser ||
                User.IsSystemAdmin || User.IsHrDataEntry)
                {
                return true;
                }
            return false;
            }

        #endregion

        #region SysRole security

        public Enums.Privilege GetSysRolePrivilege()
            {
            if(User.IsSystemAdmin)
                {
                return Enums.Privilege.FullContolPrivilege;

                }
            else if(User.HasAnyAdminRole())
                {
                return Enums.Privilege.ReadOnlyPrivilege;
                }
            return Enums.Privilege.AccessDeniedPrivilege;
            }

        public Enums.Privilege GetSysRolePrivilege(SysRole sysRole)
            {
            if(sysRole == null)
                {
                return Enums.Privilege.AccessDeniedPrivilege;
                }
            var listIds = EnumExtension.GetIntegerValues<Enums.UserRole>();
            if(sysRole.RoleId == Enums.Cnt.Na || listIds.Contains(sysRole.RoleId))
                {
                if(User.IsSystemAdmin)
                    {
                    return Enums.Privilege.ModifyPrivilege;

                    }
                return Enums.Privilege.AccessDeniedPrivilege;
                }
            if(User.IsSystemAdmin)
                {
                ServiceRepository.SysRoleRepository().LoadNavigationProperty(sysRole, sr => sr.SysUserRoles);
                if(!sysRole.SysUserRoles.Any())
                    {
                    return Enums.Privilege.FullContolPrivilege;
                    }
                return Enums.Privilege.ModifyPrivilege;
                }

            else if(User.HasAnyAdminRole())
                {
                return Enums.Privilege.ReadOnlyPrivilege;
                }
            return Enums.Privilege.AccessDeniedPrivilege;
            }
        
        public Enums.Privilege GetSysRolePrivilege(int roleId)
            {
            var sysRole = ServiceRepository.SysRoleRepository().GetSysRoleById(roleId);
            return GetSysRolePrivilege(sysRole);
            }
        #endregion

        #region Organisational level security

        public Enums.Privilege GetOrgLevelPrivilege()
            {
            if(User.IsSystemAdmin)
                {
                return Enums.Privilege.FullContolPrivilege;

                }
            else if(User.HasAdminOrPowerRole())
                {
                return Enums.Privilege.ReadOnlyPrivilege;
                }
            return Enums.Privilege.AccessDeniedPrivilege;
            }

        public Enums.Privilege GetOrgLevelPrivilege(OrgLevel orgLevel)
            {
            if(orgLevel == null)
                {
                return Enums.Privilege.AccessDeniedPrivilege;
                }

            if(orgLevel.OrgLevelId == Enums.Cnt.Na)
                {
                if(User.IsSystemAdmin)
                    {
                    return Enums.Privilege.ModifyPrivilege;

                    }
                return Enums.Privilege.AccessDeniedPrivilege;
                }
            if(User.IsSystemAdmin)
                {
                ServiceRepository.OrgLevelRepository().LoadNavigationProperty(orgLevel, ol => ol.SysUserRoles);
                if(!orgLevel.SysUserRoles.Any())
                    {
                    return Enums.Privilege.FullContolPrivilege;
                    }
                return Enums.Privilege.ModifyPrivilege;
                }

            else if(User.HasAnyAdminRole())
                {
                return Enums.Privilege.ReadOnlyPrivilege;
                }
            return Enums.Privilege.AccessDeniedPrivilege;
            }

        public Enums.Privilege GetOrgLevelPrivilege(int orgLevelId)
            {
            var orgLevel = ServiceRepository.OrgLevelRepository().GetOrgLevelById(orgLevelId);
            return GetOrgLevelPrivilege(orgLevel);
            }
        #endregion
        public bool CanEditBudget(RoleDescription rd)
            {
            var psse = Enum.GetName(typeof(Enums.GradeType), 2);


            var grade =
                ServiceRepository.GradeRepository()
                    .List()
                    .FirstOrDefault(g => g.GradeCode == rd.RolePositionDescription.GradeCode);
            if(grade.GradeType == Enums.GradeType.PSSE.ToString())
                {
                return true;
                }

            return false;
            }

        public virtual ServiceRepository ServiceRepository => _serviceRepository ?? (_serviceRepository = new ServiceRepository(RepositoryFactory));


        #region UserRole security

        public virtual Enums.Privilege GetUserRolePrivilege(Enums.UserRole role)
            {
            return Enums.Privilege.ReadOnlyPrivilege;
            }

        /// <returns></returns>
        public virtual Enums.Privilege GetUserRolePrivilege(SysUserRole sysUserRole)
            {
            if(sysUserRole == null) return Enums.Privilege.AccessDeniedPrivilege;
            return GetUserRolePrivilege((Enums.UserRole)sysUserRole.RoleId);
            }

        public virtual Enums.Privilege GetUserRolePrivilege(UserRoleModel model)
            {
            var sysRole = ServiceRepository.SysUserRoleRepository().GetSysUserRoleByModel(model);
            return GetUserRolePrivilege(sysRole);
            }


        public virtual IEnumerable<UiPropertyItem> GetPropertySettings(Enums.UserRole role, FormType formType)
        {
            var formTypeLists = new List<FormType>
            {
                FormType.Search,
                FormType.Details,
                FormType.Delete

            };
            var propList = UserRoleModel.GetAllUserRoleProperties();
            UiPropertyItem.GetProperty(propList, UserRoleModel.OrgLevelIdProp).HideProperty();
            UiPropertyItem.GetProperty(propList, UserRoleModel.RoleIdProp).HideProperty();
            UiPropertyItem.GetProperty(propList, UserRoleModel.UserIdProp).HideProperty();


            if (formTypeLists.Contains(formType))
            {
                if (GetUserRolePrivilege(role).CanRead)
                {
                    switch (role)
                    {
                        case Enums.UserRole.SystemAdministrator:

                        case Enums.UserRole.Administrator:

                        case Enums.UserRole.PowerUser:

                        case Enums.UserRole.HRDataEntry:
                            UiPropertyItem.GetProperty(propList, UserRoleModel.OrgObjetcNameProp).HideProperty();
                            UiPropertyItem.GetProperty(propList, UserRoleModel.StructureIdProp).HideProperty();
                            UiPropertyItem.GetProperty(propList, UserRoleModel.OrgLevelNameProp).HideProperty();

                            break;
                    }

                    return propList;
                }
                else
                {
                    propList.Clear();
                    return propList;
                    }
            }
            if (formType == FormType.Edit || formType == FormType.Create)
            {
                if (formType == FormType.Edit)
                {
                    UiPropertyItem.GetProperty(propList, UserRoleModel.EmailProp).PropertyAttributes += "disabled";
                   // UiPropertyItem.GetProperty(propList, UserRoleModel.StructureIdProp).PropertyAttributes += "disabled";
                }
                var structureIdProp = UiPropertyItem.GetProperty(propList, UserRoleModel.StructureIdProp);
                switch (role)
                {
                    case Enums.UserRole.SystemAdministrator:
                        
                    case Enums.UserRole.Administrator:

                    case Enums.UserRole.PowerUser:

                    case Enums.UserRole.HRDataEntry:
                        UiPropertyItem.GetProperty(propList, UserRoleModel.OrgObjetcNameProp).HideProperty();
                        UiPropertyItem.GetProperty(propList, UserRoleModel.StructureIdProp).HideProperty();
                        UiPropertyItem.GetProperty(propList, UserRoleModel.OrgLevelNameProp).HideProperty();

                        break;

                    case Enums.UserRole.DivisionEditor:
                    case Enums.UserRole.DivisionApprover:

                        structureIdProp.PropertyValueList = ServiceRepository.ExecutiveRepository().List().ToArray()
                            .Select(
                                div => new SelectListItemExtension {Value = div.ExecutiveCod, Text = div.ExecutiveTitle})
                            .ToArray();

                        break;
                    case Enums.UserRole.DirectorateDataEntry:
                    case Enums.UserRole.DirectorateEndorser:

                        structureIdProp.PropertyValueList = ServiceRepository.DirectorateRepository().List().ToArray()
                            .Select(
                                dir =>
                                    new SelectListItemExtension
                                    {
                                        Value = dir.DirectorateId.ToString(),
                                        Text = $"{dir.DirectorateName} ({dir.Executive.ExecutiveTitle})"
                                    })
                            .ToArray();

                        break;

                    case Enums.UserRole.BusinessUnitDataEntry:
                    case Enums.UserRole.BusinessUnitAuthor:

                        structureIdProp.PropertyValueList = ServiceRepository.BusinessUnitRepository().List().ToArray()
                            .Select(
                                bu =>
                                    new SelectListItemExtension
                                    {
                                        Value = bu.BUnitId.ToString(),
                                        Text =
                                            $"{bu.BUnitName}- {bu.Directorate.DirectorateName} ({bu.Directorate.Executive.ExecutiveTitle})"
                                    })
                            .ToArray();

                        break;

                }
                if (GetUserRolePrivilege(role).CanCreate || GetUserRolePrivilege(role).CanEdit)
                {
                    return propList;
                }
                else
                {
                    propList.Clear();
                    return propList;
                    }
            }
            propList.Clear();
            return propList;
            }
        public IEnumerable<Position> FilterLinkedPositions(IEnumerable<Position> allLinkedPositions)
            {
            var rep = ServiceRepository.PositionRepository();

            if(User.HasAdminOrPowerRole())
                {
                return allLinkedPositions.Where(p => p.StatusId != (int)Enums.StatusValue.Deleted);
                }
            if(User.HasAnyAdminRole())
                {
                return allLinkedPositions.Where(p => p.StatusId != (int)Enums.StatusValue.Deleted);
                }
            var stsLiveList = new int[] { (int)Enums.StatusValue.Approved, (int)Enums.StatusValue.Imported };
            return allLinkedPositions.Where(p => stsLiveList.Contains(p.StatusId));
            }

        #endregion

        protected UserTaskBase(IRepositoryFactory factory, UserInfoExtension user, Enums.UserRole role, string name, string description = Cnt.Empty)
            {
            if(user == null)
                {
                throw new ArgumentNullException(nameof(user), "Task user could not be null");
                }

            if(string.IsNullOrWhiteSpace(name))
                {
                throw new ArgumentNullException(nameof(name), "Task name could not be null nor empty");
                }

            User = user;
            Name = name;

            if(string.IsNullOrWhiteSpace(description))
                {
                description = name;
                }

            Description = description;

            RepositoryFactory = factory;
            DashboardTitle = $"{role.GetDescription()} dashboard";
            }
        }
    }

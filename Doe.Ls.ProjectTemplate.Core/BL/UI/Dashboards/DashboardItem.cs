using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.Dashboards
    {
    public class DashboardItem
        {
        public string DisplayText { get; set; }
        public string ToolTip { get; set; }
        public string Url { get; set; }
        public string GetClassName()
            {
            var items = Url.Split('/', '~');
            string className = "";
            if(items.Length > 0)
                {
                className = items.Last();
                }
            else
                {
                className = Url;
                }
            if(className.IndexOf('?') > 0)
                {
                className = className.Substring(0, className.IndexOf('?'));
                }
            if(string.IsNullOrWhiteSpace(className)) return className;
            return className.Wordify().Replace(" ", "-").ToLower();
            }
        public UiStatus Status { get; set; }

        public override string ToString()
            {
            return DisplayText;
            }

        #region Search (or browse) tasks
        public static DashboardItem PositionChart(IUserTask task)
            {
            var action = "Browse";
            var status = UiStatus.Visible;

            var item = new DashboardItem
                {
                DisplayText = $"{action} organisational chart",
                Url = $"~/Position/InitiateChartLoading?cache_id={AppCacheHelper.Token}",

                ToolTip = $"{action} organisational chart",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManagePositions(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAnyAdminRoleExceptHr())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Visible;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action} positions",
                Url = $"~/Position?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action} positions",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageRoleDescriptions(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAnyAdminRoleExceptHr())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Visible;
                }

            var item = new DashboardItem
                {
                DisplayText = $"{action} role descriptions <span class='hint-dash'>(Public Service positions)</span>",
                Url = $"~/RoleDescription?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action} role descriptions-Public Service positions",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManagePositionDescriptions(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAnyAdminRoleExceptHr())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Visible;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action} position descriptions <span class='hint-dash'>(Teaching Service positions)</span>",
                Url = $"~/PositionDescription?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  position descriptions",
                Status = status
                };

            return item;
            }

        #endregion

        #region Organisational structure tasks
        public static DashboardItem ManageExecutives(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Visible;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  divisions",
                Url = $"~/Executive?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  divisions",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageDirectorates(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Visible;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  directorates",
                Url = $"~/Directorate?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  directorates",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageFunctionalAreas(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Visible;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  functional areas",
                Url = $"~/FunctionalArea?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  functional areas",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageBusinessUnits(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Visible;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  business units",
                Url = $"~/BusinessUnit?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  business units",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageTeams(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Visible;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  teams ",
                Url = $"~/Unit?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  teams",
                Status = status
                };

            return item;
            }



        public static DashboardItem ManageLocations(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Visible;
                }

            var item = new DashboardItem
                {
                DisplayText = $"{action}  locations ",
                Url = $"~/Location?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  locations",
                Status = status
                };

            return item;
            }

        #endregion

        #region Positions tasks
        public static DashboardItem ManageCapabilityGroups(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  capability groups",
                Url = $"~/CapabilityGroup?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  capability groups",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManagePositionType(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  position types ",
                Url = $"~/PositionType?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  position types",
                Status = status
                };
            return item;

            }

        public static DashboardItem ManagePositionStatus(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }

            var item = new DashboardItem
                {
                DisplayText = $"{action}  position statuses ",
                Url = $"~/PositionStatusValue?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  position statuses",
                Status = status
                };
            return item;

            }

        public static DashboardItem ManagePositionLevel(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }

            var item = new DashboardItem
                {
                DisplayText = $"{action}  position Levels ",
                Url = $"~/PositionLevel?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  position levels",
                Status = status
                };
            return item;

            }

        public static DashboardItem ManageEmployeeType(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  employee types ",
                Url = $"~/EmployeeType?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  employee types",
                Status = status
                };
            return item;

            }

        #endregion

        #region Role descriptions tasks
        public static DashboardItem RoleDescCapabilityMatrix(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }

            var item = new DashboardItem
                {
                DisplayText = $"{action}  role desc capability matrix",
                Url = $"~/RoleDescCapabilityMatrix?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  role desc capability matrix",
                Status = status
                };

            return item;


            }

        public static DashboardItem ManageCapabilityBehaviourIndicators(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  capability behavioural indicators",
                Url = $"~/CapabilityBehaviourIndicator?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  capability behavioural indicators",
                Status = status
                };
            return item;
            }

        public static DashboardItem ManageCapabilityLevels(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  capability levels",
                Url = $"~/CapabilityLevel?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  capability levels",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageCapabilityNames(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  capability names",
                Url = $"~/CapabilityName?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  capability names",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageCapabilityGroup(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  capability groups",
                Url = $"~/CapabilityGroup?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  capability groups",
                Status = status
                };
            return item;

            }

        public static DashboardItem ManageGrade(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  grade",
                Url = $"~/Grade?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  grade",
                Status = status
                };
            return item;

            }
        #endregion

        #region Position descriptions tasks
        public static DashboardItem ManageLookupFocusGradeCriterias(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else if(task.User.HasAnyAdminRole())
                {
                action = "Browse";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Visible;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  lookup focus grade criteria",
                Url = $"~/LookupFocusGradeCriteria?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  lookup focus grade criteria",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageFocuses(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  focuses ",
                Url = $"~/Focus?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  focuses",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageSelectionCriterias(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  selection criteria ",
                Url = $"~/SelectionCriteria?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  selection criteria",
                Status = status
                };

            return item;
            }
        #endregion

        #region User roles and security tasks
        public static DashboardItem ManageOrgLevel(IUserTask task)
            {
            string action = "";
            UiStatus status = UiStatus.Hidden;
            if(task.User.IsSystemAdmin)
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Browse";
                status = UiStatus.Visible;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  organisational security levels",
                Url = $"~/OrgLevel?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  organisational security levels",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageRolesTask(IUserTask task)
            {
            string action = "";
            UiStatus status = UiStatus.Hidden;
            if(task.User.IsSystemAdmin)
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Browse";
                status = UiStatus.Visible;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  system roles",
                Url = $"~/SysRole?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  system roles",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageSystemRoleUsersTask(IUserTask task)
            {
            var priv = task.GetUserRolePrivilege(Enums.UserRole.SystemAdministrator);
            return GetItem(priv, Enums.UserRole.SystemAdministrator);
            }
        public static DashboardItem ManageAdministratorRoleUsersTask(IUserTask task)
            {
            var priv = task.GetUserRolePrivilege(Enums.UserRole.Administrator);
            return GetItem(priv, Enums.UserRole.Administrator);

            }
        public static DashboardItem ManagePowerUserRoleUsersTask(IUserTask task)
            {
            var priv = task.GetUserRolePrivilege(Enums.UserRole.PowerUser);
            return GetItem(priv, Enums.UserRole.PowerUser);
            }
        public static DashboardItem ManageBusinessUnitDataEntryRoleUsersTask(IUserTask task)
            {
            var priv = task.GetUserRolePrivilege(Enums.UserRole.BusinessUnitDataEntry);
            return GetItem(priv, Enums.UserRole.BusinessUnitDataEntry);
            }
        public static DashboardItem ManageBusinessUnitAutherRoleUsersTask(IUserTask task)
            {
            var priv = task.GetUserRolePrivilege(Enums.UserRole.BusinessUnitAuthor);
            return GetItem(priv, Enums.UserRole.BusinessUnitAuthor);
            }

        public static DashboardItem ManageDirectorateDataEntryRoleUsersTask(IUserTask task)
            {
            var priv = task.GetUserRolePrivilege(Enums.UserRole.DirectorateDataEntry);
            return GetItem(priv, Enums.UserRole.DirectorateDataEntry);
            }
        public static DashboardItem ManageDirectorateEndorserRoleUsersTask(IUserTask task)
            {
            var priv = task.GetUserRolePrivilege(Enums.UserRole.DirectorateEndorser);
            return GetItem(priv, Enums.UserRole.DirectorateEndorser);
            }

        public static DashboardItem ManageDivisionEditorRoleUsersTask(IUserTask task)
            {
            var priv = task.GetUserRolePrivilege(Enums.UserRole.DivisionEditor);
            return GetItem(priv, Enums.UserRole.DivisionEditor);
            }
        public static DashboardItem ManageDivisionApproverRoleUsersTask(IUserTask task)
            {
            var priv = task.GetUserRolePrivilege(Enums.UserRole.DivisionApprover);
            return GetItem(priv, Enums.UserRole.DivisionApprover);
            }

        public static DashboardItem ManageHrRoleUsersTask(IUserTask task)
            {
            var priv = task.GetUserRolePrivilege(Enums.UserRole.HRDataEntry);
            return GetItem(priv, Enums.UserRole.HRDataEntry);
            }
        public static DashboardItem ManageUsersTask(IUserTask task)
            {
            var action = "";
            var status = UiStatus.Hidden;
            if(task.User.IsSystemAdmin)
                {
                action = "Lookup";
                status = UiStatus.Visible;
                }
            else if(task.User.HasAnyAdminRole())
                {
                action = "Lookup";
                status = UiStatus.Visible;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  users ",
                Url = $"~/User?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  users",
                Status = status
                };

            return item;
            }

        #endregion

        #region Application configuration tasks
        public static DashboardItem SysAppMessageTask(IUserTask task)
            {
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {

                status = UiStatus.Visible;
                }
            else
                {

                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = "Manage app messages",
                Url = $"~/sysMessage?cache_id={AppCacheHelper.Token}",
                ToolTip = "Manage app messages",
                Status = status
                };

            return item;
            }
        public static DashboardItem ManageApplicationLogsTask(IUserTask task)
            {
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {

                status = UiStatus.Visible;
                }
            else
                {

                status = UiStatus.Visible;
                }
            var item = new DashboardItem
                {
                DisplayText = "Access application logs",
                Url = $"~/GeneralLog?cache_id={AppCacheHelper.Token}",
                ToolTip = "Access application logs",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageRelationshipScopeTask(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  relationship scopes",
                Url = $"~/RelationshipScope?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  relationship scopes",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageEmployeesTask(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  employees (coming soon)",
                Url = "#?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  employees (coming soon)",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageOccupationsTask(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  occupation types",
                Url = $"~/OccupationType?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  occupation types",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageGradeTask(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  grade",
                Url = $"~/Grade?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  grade",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageGlobalSettingsTask(IUserTask task)
            {
            string action;
            UiStatus status;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            else
                {
                action = "Browse";
                status = UiStatus.Hidden;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action}  global items",
                Url = $"~/GlobalItem?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action}  global items",
                Status = status
                };

            return item;
            }

        public static DashboardItem ManageWorkflowActionTask(IUserTask task)
            {
            string action = "Browse";
            UiStatus status = UiStatus.Hidden;
            if(task.User.HasAdminOrPowerRole())
                {
                action = "Manage";
                status = UiStatus.Visible;
                }
            var item = new DashboardItem
                {
                DisplayText = $"{action} workflow actions",
                Url = $"~/WFAction?cache_id={AppCacheHelper.Token}",
                ToolTip = $"{action} workflow actions",
                Status = status
                };

            return item;
            }

        #endregion

        #region Workflow tasks
        public static DashboardItem PositionsApprovalTask(IUserTask task)
            {
            var status = task.User.HasApprovalRole() ? UiStatus.Visible : UiStatus.Hidden;

            var item = new DashboardItem
                {
                DisplayText = "Positions workflow approval tasks",
                Url = $"~/Position/PositionApprovalList?cache_id={AppCacheHelper.Token}",
                ToolTip = "Positions workflow approval tasks",
                Status = status
                };
            return item;
            }
        #endregion

        private static string GetLinkText(Enums.Privilege privilege, Enums.UserRole role)
            {
            var action = "";
            if(privilege.CanCreate) action = "Manage";
            else if(privilege.CanRead) action = "Browse";

            return $"{action} {role.GetDescription().ToLower()} user list";
            }
        private static UiStatus GetLinkStatus(Enums.Privilege privilege)
            {
            var status = UiStatus.Hidden;
            if(privilege.CanCreate || privilege.CanRead) status = UiStatus.Visible;
            return status;
            }

        private static DashboardItem GetItem(Enums.Privilege privilege, Enums.UserRole role, string serverUrl)
            {
            var text = GetLinkText(privilege, role);
            return new DashboardItem
                {
                Status = GetLinkStatus(privilege),
                DisplayText = text,
                ToolTip = text,
                Url = serverUrl
                };
            }
        private static DashboardItem GetItem(Enums.Privilege privilege, Enums.UserRole role)
            {
            return GetItem(privilege, role, GetDashboardRoleItemLink(role));
            }

        public static string GetDashboardRoleItemLink(Enums.UserRole role)
            {
            return $"~/UserRole/List{role}Users?cache_id={AppCacheHelper.Token}";
            }


        public static DashboardItem ExpireMessageListCacheTask(IUserTask task)
            {
            return GetSysTask("ExpireMessageListCache", task);
            }

        public static DashboardItem ExpirePositionChartCacheTask(IUserTask task)
            {
            return GetSysTask("ExpirePositionChartCache", task);
            }

        public static DashboardItem ExpireAllCacheTask(IUserTask task)
            {
            return GetSysTask("ExpireAllCache", task);
            }

        public static DashboardItem ExpireSessionTask(IUserTask task)
            {
            return GetSysTask("ExpireSession", task);
            }

        public static DashboardItem ResetApplicationTask(IUserTask task)
            {
            return GetSysTask("ResetApplication", task);
            }

        public static DashboardItem UpdateAllhierarchy(IUserTask task)
            {
            return GetSysTask("UpdateAllHierarchy", task);
            }

        public static DashboardItem GetSysTask(string action, IUserTask task)
            {
            var text = action.Wordify();
            return new DashboardItem
                {
                Status = (task.User.HasAdminOrPowerRole()) ? UiStatus.Visible : UiStatus.Hidden,
                DisplayText = text,
                ToolTip = text,
                Url = $"~/sys/{action}"
                };

            }
        }



    }

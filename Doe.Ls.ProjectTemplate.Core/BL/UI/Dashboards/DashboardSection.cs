using System.Collections.Generic;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.Dashboards
{
    public class DashboardSection
    {
        public string Title { get; set; }
        public List<DashboardItem> DashboardItems { get; set; }
        public UiStatus Status { get; set; }
        public override string ToString() {
            return Title;
        }
        public static DashboardSection SearchAndBrowseTasks(IUserTask task) {
            var section = new DashboardSection() {
                Title = "Search (or browse) tasks",
                DashboardItems = new List<DashboardItem>()
                {
                    DashboardItem.PositionChart(task),
                    DashboardItem.ManagePositions(task),
                    //DashboardItem.ManageRoleDescriptions(task),
                    DashboardItem.ManagePositionDescriptions(task),
                }
            };
            if (!task.User.IsGuest) section.Status = UiStatus.Visible;
            return section;
        }

        public static DashboardSection OrganisationalStructureTasks(IUserTask task) {
            var section = new DashboardSection() {
                Title = "Organisational structure tasks",
                DashboardItems = new List<DashboardItem>()
                {
                    DashboardItem.ManageExecutives(task),
                    DashboardItem.ManageDirectorates(task),
                    DashboardItem.ManageFunctionalAreas(task),
                    DashboardItem.ManageBusinessUnits(task),
                    DashboardItem.ManageTeams(task),
                    DashboardItem.ManageLocations(task),
                }
            };
            if (task.User.HasAnyAdminRole() || task.User.IsHrDataEntry) { section.Status = UiStatus.Visible; } else { section.Status = UiStatus.Hidden; }

            return section;
        }

        public static DashboardSection PositionsTasks(IUserTask task) {
            var section = new DashboardSection {
                Title = "Positions tasks",
                DashboardItems = new List<DashboardItem>()
                {
                    DashboardItem.ManagePositions(task),
                    DashboardItem.ManageCapabilityGroups(task),
                    DashboardItem.ManagePositionType(task),
                    DashboardItem.ManageEmployeeType(task),
                    DashboardItem.ManagePositionStatus(task),
                    DashboardItem.ManageEmployeesTask(task),
                    DashboardItem.ManageGrade(task),
                    DashboardItem.ManagePositionLevel(task),
                },
                Status = task.User.HasAdminOrPowerRole() ? UiStatus.Visible : UiStatus.Hidden
            };

            return section;

        }

        public static DashboardSection RoleDescriptionsTasks(IUserTask task) {
            var section = new DashboardSection {
                Title = "Role descriptions tasks",
                DashboardItems = new List<DashboardItem>()
                {
                    DashboardItem.ManageRoleDescriptions(task),
                    DashboardItem.ManageCapabilityGroups(task),
                    DashboardItem.ManageCapabilityNames(task),
                    DashboardItem.ManageCapabilityLevels(task),
                    DashboardItem.ManageCapabilityBehaviourIndicators(task),
                    DashboardItem.RoleDescCapabilityMatrix(task),
                    DashboardItem.ManageGrade(task),
                },
                Status = task.User.HasAdminOrPowerRole() ? UiStatus.Visible : UiStatus.Hidden
            };
            return section;
        }

        public static DashboardSection PositionDescriptionsTasks(IUserTask task) {
            var section = new DashboardSection {
                Title = "Position descriptions tasks",
                DashboardItems = new List<DashboardItem>()
                {
                    DashboardItem.ManagePositionDescriptions(task),
                    DashboardItem.ManageSelectionCriterias(task),
                    DashboardItem.ManageFocuses(task),
                    DashboardItem.ManageGrade(task),
                    DashboardItem.ManageLookupFocusGradeCriterias(task),
                },
                Status = task.User.HasAdminOrPowerRole() ? UiStatus.Visible : UiStatus.Hidden
            };
            return section;
        }

        public static DashboardSection UserRolesAndSecurityTasks(IUserTask task) {
            var section = new DashboardSection() {
                Title = "User roles and security tasks",
                DashboardItems = new List<DashboardItem>()
                {
                    //DashboardItem.ManageRolesTask(task),
                    //DashboardItem.ManageOrgLevel(task),

                    DashboardItem.ManageUsersTask(task),

                    DashboardItem.ManageSystemRoleUsersTask(task),
                    DashboardItem.ManageAdministratorRoleUsersTask(task),
                    DashboardItem.ManagePowerUserRoleUsersTask(task),

                    //DashboardItem.ManageDivisionApproverRoleUsersTask(task),
                    DashboardItem.ManageDivisionEditorRoleUsersTask(task),


                    //DashboardItem.ManageDirectorateEndorserRoleUsersTask(task),
                    //DashboardItem.ManageDirectorateDataEntryRoleUsersTask(task),

                    //DashboardItem.ManageBusinessUnitAutherRoleUsersTask(task),
                    //DashboardItem.ManageBusinessUnitDataEntryRoleUsersTask(task),
                    
                    //DashboardItem.ManageHrRoleUsersTask(task),


                   }
            };
            if (task.User.HasAnyAdminRole()) { section.Status = UiStatus.Visible; } else { section.Status = UiStatus.Hidden; }
            return section;
        }

        public static DashboardSection ApplicationConfigurationTasks(IUserTask task) {
            var section = new DashboardSection() {
                Title = "Application configuration tasks",
                DashboardItems = new List<DashboardItem>()
                {
                    DashboardItem.SysAppMessageTask(task),

                    //DashboardItem.ManageGradeTask(task),
                    //DashboardItem.ManageOccupationsTask(task),

                    DashboardItem.ManageApplicationLogsTask(task),
                  
                    //DashboardItem.ManageRelationshipScopeTask(task),
                    //DashboardItem.ManageWorkflowActionTask(task),

                    DashboardItem.ManageGlobalSettingsTask(task),

                   }
            };
            if (task.User.HasAdminOrPowerRole()) { section.Status = UiStatus.Visible; } else { section.Status = UiStatus.Hidden; }
            return section;
        }

        public static DashboardSection WorkflowTasks(IUserTask task) {
            var section = new DashboardSection() {
                Title = "Workflow tasks",
                DashboardItems = new List<DashboardItem>()
                    {
                    DashboardItem.PositionsApprovalTask(task),
                    }
            };
            if (task.User.HasApprovalRole()) { section.Status = UiStatus.Visible; } else { section.Status = UiStatus.Hidden; }
            return section;
        }

        public static DashboardSection SystemTasks(IUserTask task) {
            var section = new DashboardSection() {
                Title = "System tasks",
                DashboardItems = new List<DashboardItem>()
                {
                    DashboardItem.ExpireAllCacheTask(task),
                    DashboardItem.ExpireSessionTask(task),

                    DashboardItem.ExpireMessageListCacheTask(task),
                    DashboardItem.ExpirePositionChartCacheTask(task),

                    DashboardItem.ResetApplicationTask(task),
                    //DashboardItem.UpdateAllhierarchy(task),
                    
                   }
            };
            section.Status = task.User.HasAdminOrPowerRole() ? UiStatus.Visible : UiStatus.Hidden;
            return section;
        }
    }

}
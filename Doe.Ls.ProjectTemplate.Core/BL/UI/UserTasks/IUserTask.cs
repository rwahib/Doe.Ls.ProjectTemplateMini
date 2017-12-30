using System.Collections.Generic;
using System.Linq;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.Dashboards;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks
    {
    public interface IUserTask
        {
        IRepositoryFactory RepositoryFactory { get; set; }

        UserInfoExtension User { get; }
        string Name { get; }
        string Description { get; }

        Dashboard Dashboard { get; }
        Enums.Privilege GetSysUserPrivilege(string userId);
        bool CanApproveDocuments();
        bool CanAccessOrganisationalStructure();
        bool CanAccessApplicationConfiguration();
        bool CanAccessRoleDescriptionTasks();
        
        bool CanAccessUserRoleAndSecurityTasks();
        RolePosPropertyStateModel GetRolePosModelState(int id);
        bool CanEditPositionDescription(int positionDescriptionId);
        
        bool CanDownloadRoleDesc(int roleDescriptionId);

        bool HasLinkedPositions(int rolePositionDescId);
        bool CanEditNotes(PositionInformation positionInfo, PositionNote currentNote);
        bool CanViewRolePositiondesc();
        bool CanEditBudget(RoleDescription rd);
        bool CanEditRoleDesc(int rolePositionDescId);

        Enums.Privilege GetWorkflowObjectPrivilege(WorkflowObjectType workflowObjectType);

        #region Executive security

        Enums.Privilege GetExecutivePrivilege();

        Enums.Privilege GetExecutivePrivilege(Executive executive);

        Enums.Privilege GetExecutivePrivilege(string executiveCode);

        #endregion

        #region Directorate security

        Enums.Privilege GetDirectoratePrivilege();

        Enums.Privilege GetDirectoratePrivilege(Directorate directorate);

        Enums.Privilege GetDirectoratePrivilege(int directorateId);

        #endregion

        #region Business unit security
        Enums.Privilege GetBUnitPrivilege();
        Enums.Privilege GetBUnitPrivilege(BusinessUnit bUnit);
        Enums.Privilege GetBUnitPrivilege(int bUnitId);
        #endregion

        #region Function area security
        Enums.Privilege GetFunctionAreaPrivilege();
        Enums.Privilege GetFunctionAreaPrivilege(FunctionalArea functionalArea);
        Enums.Privilege GetFunctionAreaPrivilege(int funcationalAreaId);
        #endregion

        #region Team security
        Enums.Privilege GetTeamPrivilege();
        Enums.Privilege GetTeamPrivilege(Unit unit);
        Enums.Privilege GetTeamPrivilege(int unitid);
        #endregion

        #region Location security
        Enums.Privilege GetLocationPrivilege();
        Enums.Privilege GetLocationPrivilege(Location location);
        Enums.Privilege GetLocationPrivilege(int locationid);
        #endregion

        #region Grade security
        Enums.Privilege GetGradePrivilege();
        Enums.Privilege GetGradePrivilege(Grade grade);
        Enums.Privilege GetGradePrivilege(string gradeCode);
        #endregion

        #region PositionLevel security
        Enums.Privilege GetPositionLevelPrivilege();
        Enums.Privilege GetPositionLevelPrivilege(PositionLevel positionLevel);
        Enums.Privilege GetPositionLevelPrivilege(int positionLevelid);
        #endregion
        
        #region Position
        // other methods are in workflow
        Enums.Privilege GetPositionPrivilege();
        #endregion

        #region Position & Role description
        // other methods are in workflow
        Enums.Privilege GetPositionDescriptionPrivilege();
        Enums.Privilege GetRoleDescriptionPrivilege();
        #endregion

        #region Sysrole
        Enums.Privilege GetSysRolePrivilege();
        Enums.Privilege GetSysRolePrivilege(SysRole sysRole);
        Enums.Privilege GetSysRolePrivilege(int sysRoleId);
        #endregion

        #region OrgLevel
        Enums.Privilege GetOrgLevelPrivilege();
        Enums.Privilege GetOrgLevelPrivilege(OrgLevel orgLevel);
        Enums.Privilege GetOrgLevelPrivilege(int orgLevelId);
        #endregion

        IQueryable<Position> FilterPositions(IQueryable<Position> positions);
        IQueryable<Position> GetApprovalPositions();
        List<Enums.StatusValue> GetPositionStatus();
        IQueryable<Executive> GetDivisionList();
        IQueryable<PositionDescription> GetPositionDescriptions();
        IQueryable<RoleDescription> GetRoleDescriptions();

        bool CanViewStatus();

        #region UserRole security
        Enums.Privilege GetUserRolePrivilege(SysUserRole sysUserRole);
        Enums.Privilege GetUserRolePrivilege(Enums.UserRole role);
        Enums.Privilege GetUserRolePrivilege(UserRoleModel model);
        IEnumerable<UiPropertyItem> GetPropertySettings(Enums.UserRole role,FormType formType);

        #endregion

        IEnumerable<Position> FilterLinkedPositions(IEnumerable<Position> allLinkedPositions);
        }
    }

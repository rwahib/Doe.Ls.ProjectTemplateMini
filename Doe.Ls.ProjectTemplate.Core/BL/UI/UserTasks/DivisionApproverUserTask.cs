using System;
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
    public class DivisionApproverUserTask : UserTaskBase
        {
        public DivisionApproverUserTask(IRepositoryFactory factory, UserInfoExtension user) :
            base(factory, user, Enums.UserRole.DivisionApprover, Enums.UserRole.DivisionApprover.ToString(), Enums.UserRole.DivisionApprover.GetDescription())
            {
            }
        public override Enums.Privilege GetUserRolePrivilege(Enums.UserRole role)
            {
            switch(role)
                {
                case Enums.UserRole.SystemAdministrator:
                    return Enums.Privilege.AccessDeniedPrivilege;
                

                case Enums.UserRole.BusinessUnitDataEntry:

                case Enums.UserRole.BusinessUnitAuthor:

                case Enums.UserRole.DirectorateDataEntry:

                case Enums.UserRole.DirectorateEndorser:

                case Enums.UserRole.DivisionEditor:

                case Enums.UserRole.DivisionApprover:

                case Enums.UserRole.HRDataEntry:

                    return Enums.Privilege.FullContolPrivilege;
                }
            return Enums.Privilege.ReadOnlyPrivilege;
            }

        public override IQueryable<Position> GetApprovalPositions()
            {

            var divisionCodes = User.ActiveRoleOrgLevelList.Where(r => r.IsActive && r.RoleId == (int)Enums.UserRole.DivisionApprover).Select(r => r.StructureId);

            return FilterPositions(ServiceRepository.PositionRepository().List()).Where(p => p.StatusId == (int)Enums.StatusValue.Endorsed && divisionCodes.Contains(p.Unit.BusinessUnit.Directorate.ExecutiveCod));

            }
        }
    }
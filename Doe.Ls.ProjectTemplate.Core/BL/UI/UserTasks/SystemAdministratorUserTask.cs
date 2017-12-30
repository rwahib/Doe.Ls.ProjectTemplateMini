using System;
using System.Collections.Generic;
using System.Linq;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks
{
    public class SystemAdministratorUserTask : UserTaskBase
    {
        public SystemAdministratorUserTask(IRepositoryFactory factory, UserInfoExtension user) :
            base(
            factory, user, Enums.UserRole.SystemAdministrator, Enums.UserRole.SystemAdministrator.ToString(),
            Enums.UserRole.SystemAdministrator.GetDescription())
        {
        }
        

        public override Enums.Privilege GetUserRolePrivilege(Enums.UserRole role)
        {
            return Enums.Privilege.FullContolPrivilege;

            }
        public override IQueryable<Position> GetApprovalPositions()
            {
            return FilterPositions(ServiceRepository.PositionRepository().List());
            }
        }
}


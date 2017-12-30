using System;
using System.Collections.Generic;
using System.Linq;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.Dashboards;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks
    {
    public class BusinessUnitAuthorUserTask : UserTaskBase
        {
        public BusinessUnitAuthorUserTask(IRepositoryFactory factory, UserInfoExtension user) :
            base(factory, user, Enums.UserRole.PowerUser, Enums.UserRole.BusinessUnitAuthor.ToString(), Enums.UserRole.BusinessUnitAuthor.GetDescription())
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
                    return Enums.Privilege.FullContolPrivilege;
                }
            return Enums.Privilege.ReadOnlyPrivilege;
            }
        

        public override IQueryable<Position> GetApprovalPositions()
            {

            var buIds = User.ActiveRoleOrgLevelList.Where(r => r.IsActive && r.RoleId == (int)Enums.UserRole.BusinessUnitAuthor).Select(r => r.StructureId).CastToIntegerList();
            
            return FilterPositions(ServiceRepository.PositionRepository().List()).Where(p => p.StatusId == (int)Enums.StatusValue.Draft && buIds.Contains(p.Unit.BUnitId));
            }

        }
    }
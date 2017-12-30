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
    public class DirectorateEndorserUserTask : UserTaskBase
    {
        public DirectorateEndorserUserTask(IRepositoryFactory factory, UserInfoExtension user) :
            base(factory, user, Enums.UserRole.DirectorateEndorser, Enums.UserRole.DirectorateEndorser.ToString(), Enums.UserRole.DirectorateEndorser.GetDescription())
        {
        }
        public override Enums.Privilege GetUserRolePrivilege(Enums.UserRole role)
            {
            switch(role)
                {
                case Enums.UserRole.SystemAdministrator:
                    return Enums.Privilege.AccessDeniedPrivilege;
                    
                case Enums.UserRole.DirectorateEndorser:
                    return Enums.Privilege.FullContolPrivilege;

                case Enums.UserRole.DirectorateDataEntry:
                    return Enums.Privilege.FullContolPrivilege;
                    

                case Enums.UserRole.BusinessUnitDataEntry:
                    return Enums.Privilege.FullContolPrivilege;

                case Enums.UserRole.BusinessUnitAuthor:
                    return Enums.Privilege.FullContolPrivilege;

                }
            return Enums.Privilege.ReadOnlyPrivilege;
            }
        
        public override IQueryable<Position> GetApprovalPositions()
            {

            var directorateIds = User.ActiveRoleOrgLevelList.Where(r => r.IsActive && r.RoleId == (int)Enums.UserRole.DirectorateEndorser).Select(r => r.StructureId).CastToIntegerList();
            
            return FilterPositions(ServiceRepository.PositionRepository().List()).Where(p=>p.StatusId==(int)Enums.StatusValue.Submitted&& directorateIds.Contains(p.Unit.BusinessUnit.DirectorateId));
            }
        }
}
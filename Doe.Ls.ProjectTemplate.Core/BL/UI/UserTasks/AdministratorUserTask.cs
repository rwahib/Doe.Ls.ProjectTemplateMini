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
    public class AdministratorUserTask : UserTaskBase
        {
        public AdministratorUserTask(IRepositoryFactory factory, UserInfoExtension user) :
            base(factory, user, Enums.UserRole.Administrator, Enums.UserRole.Administrator.ToString(), Enums.UserRole.Administrator.GetDescription())
            {
            }
        
        public override Enums.Privilege GetUserRolePrivilege(Enums.UserRole role)
            {
            switch(role)
                {
                case Enums.UserRole.SystemAdministrator:
                    return Enums.Privilege.ReadOnlyPrivilege;
                case Enums.UserRole.Administrator:

                case Enums.UserRole.PowerUser:

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
            return FilterPositions(ServiceRepository.PositionRepository().List());
        }
        }
    }
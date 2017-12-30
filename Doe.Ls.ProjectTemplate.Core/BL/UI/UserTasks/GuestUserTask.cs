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
    public class GuestUserTask : UserTaskBase
    {
        public GuestUserTask(IRepositoryFactory factory, UserInfoExtension user) :
            base(factory, user, Enums.UserRole.Guest, Enums.UserRole.Guest.ToString(), Enums.UserRole.Guest.GetDescription())
        {
        }

        public override Enums.Privilege GetUserRolePrivilege(Enums.UserRole role)
        {
            return Enums.Privilege.AccessDeniedPrivilege;
            }
        

        public override IEnumerable<UiPropertyItem> GetPropertySettings(Enums.UserRole role, FormType formType)
            {
            return new List<UiPropertyItem>();
            }
        public override IQueryable<Position> GetApprovalPositions()
            {
            var msg = MessageHelper.AccessDeniedMessage();
            throw new InvalidOperationException(msg);
            }
        }
}
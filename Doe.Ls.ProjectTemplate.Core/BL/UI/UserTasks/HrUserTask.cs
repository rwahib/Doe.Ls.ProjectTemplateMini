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
    public class HrUserTask : UserTaskBase
    {
        public HrUserTask(IRepositoryFactory factory, UserInfoExtension user) :
            base(
            factory, user, Enums.UserRole.DoEUser, Enums.UserRole.HRDataEntry.ToString(),
            Enums.UserRole.DoEUser.GetDescription())
        {

        }

        public override Enums.Privilege GetUserRolePrivilege(Enums.UserRole role)
        {
            switch (role)
            {
                case Enums.UserRole.Administrator:

                case Enums.UserRole.PowerUser:

                case Enums.UserRole.HRDataEntry:
                    return Enums.Privilege.ReadOnlyPrivilege;
                }

            return Enums.Privilege.AccessDeniedPrivilege;

            }

        public override Enums.Privilege GetUserRolePrivilege(SysUserRole sysUserRole)
            {
            if(sysUserRole == null) return Enums.Privilege.AccessDeniedPrivilege;
            return Enums.Privilege.AccessDeniedPrivilege;
            }
        public override IEnumerable<UiPropertyItem> GetPropertySettings(Enums.UserRole role, FormType formType)
            {
            var formTypeLists = new List<FormType>
            {
                FormType.Search,
                FormType.Details,
                FormType.Delete

            };
            if(formTypeLists.Contains(formType)) return base.GetPropertySettings(role, formType);


            return new List<UiPropertyItem>();
            }
        public override IQueryable<Position> GetApprovalPositions()
            {
            var msg = MessageHelper.AccessDeniedMessage();
            throw new InvalidOperationException(msg);

            }
        }
}
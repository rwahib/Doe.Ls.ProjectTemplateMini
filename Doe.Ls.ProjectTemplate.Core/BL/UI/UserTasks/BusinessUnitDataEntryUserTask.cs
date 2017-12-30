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
    public class BusinessUnitDataEntryUserTask : UserTaskBase
        {
        public BusinessUnitDataEntryUserTask(IRepositoryFactory factory, UserInfoExtension user) :
            base(factory, user, Enums.UserRole.BusinessUnitDataEntry, Enums.UserRole.BusinessUnitDataEntry.ToString(), Enums.UserRole.BusinessUnitDataEntry.GetDescription())
            {
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
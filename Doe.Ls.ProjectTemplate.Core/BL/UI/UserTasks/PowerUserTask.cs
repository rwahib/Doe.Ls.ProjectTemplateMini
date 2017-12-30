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
    public class PowerUserTask : UserTaskBase
        {
        public PowerUserTask(IRepositoryFactory factory, UserInfoExtension user) :
            base(factory, user, Enums.UserRole.PowerUser, Enums.UserRole.PowerUser.ToString(), Enums.UserRole.PowerUser.GetDescription())
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
            return FilterPositions(ServiceRepository.PositionRepository().List());
            }

        }
    }
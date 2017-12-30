using System;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks
    {
    public class UserTaskFactory
        {
        public static IUserTask GetTask(UserInfoExtension user, IRepositoryFactory factory)
            {
            if(user == null || factory == null) return null;

            if(user.IsSystemAdmin)
                {
                return new SystemAdministratorUserTask(factory, user);
                }

            if(user.IsAdministrator)
                {
                return new AdministratorUserTask(factory, user);
                }

            if(user.IsPowerUser)
                {
                return new PowerUserTask(factory, user);
                }


            if(user.IsDivisionApprover)
                {
                return new DivisionApproverUserTask(factory, user);
                }

            if(user.IsDivisionEditor)
                {
                return new DivisionEditorUserTask(factory, user);
                }

            if(user.IsDirectorateEndorser)
                {
                return new DirectorateEndorserUserTask(factory, user);
                }

            if(user.IsDirectorateDataEntry)
                {
                return new DirectorateDataEntryUserTask(factory, user);
                }

            if(user.IsBusinessUnitAuthor)
                {
                return new BusinessUnitAuthorUserTask(factory, user);
                }

            if(user.IsBusinessUnitDataEntry)
                {
                return new BusinessUnitDataEntryUserTask(factory, user);
                }

            if(user.IsDoEUser)
                {
                return new DoEUserTask(factory, user);
                }

            if(user.IsHrDataEntry)
                {
                return new HrUserTask(factory, user);
                }

            if(user.IsGuest)
                {
                return new GuestUserTask(factory, user);
                }
            throw new InvalidOperationException($"Invalid task for this user {user.UserName}");

            }
        }
    }

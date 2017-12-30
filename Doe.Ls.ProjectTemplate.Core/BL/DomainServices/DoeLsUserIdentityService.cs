using System;
using System.Collections.Generic;
using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.DoeClients;
using Doe.Ls.ProjectTemplate.Core.VleWsUserInformation;
using Unity.Attributes;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices
{
    public class DecLsUserIdentityService : IUserIdentityService
    {
        private readonly UserInfoProxy _proxy = new UserInfoProxy();

        public UserInfo GetUserByPortalId(string id)
        {
            var result = _proxy.GetDECNSWUserByPortalID(id);

            return result == null ? null : GetUserInfoForDecUser(result);
        }

        private UserInfoExtension GetUserInfoForDecUser(DECUser decUser)
        {
            var userInfo = new UserInfoExtension
            {
                DisplayName = decUser.SimpleDisplayName,
                Email = decUser.Email,
                Title = decUser.Title,
                UserName = decUser.SimpleDECUserName,
                SurName = decUser.Surname,
                FirstName = decUser.GivenName,
                SchoolId = decUser.SchoolId,
                SchoolName = decUser.SchoolName,
                Phone = decUser.PhoneNumber,
                Groups = decUser.Groups,
                DepartmentId = decUser.DeptId,
                DepartmentName = decUser.DepatName,                
                };         

            return userInfo;
        }


        public UserInfo GetUserByEmail(string email)
        {

            var result = _proxy.GetDECNSWUserByEmailID(email);
            return result == null ? null : GetUserInfoForDecUser(result);
        }

        [Unity.Attributes.Dependency]
        public ILoggerService LoggerService { get; set; }
        [Unity.Attributes.Dependency]
        public IRepositoryFactory RepositoryFactory { get; set; }
        [Unity.Attributes.Dependency]
        public ISessionService SessionService { get; set; }

        public void Dispose()
        {

        }
    }
}
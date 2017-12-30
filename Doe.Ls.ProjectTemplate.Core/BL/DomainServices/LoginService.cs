using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;

using Unity.Attributes;
using Cnt = Doe.Ls.EntityBase.Cnt;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices
    {
    public class LoginService : ILoginService
        {
        public UserInfoExtension GetUser(string userName)
            {
            userName = userName.Trim().ToLower();
            if(string.IsNullOrWhiteSpace(userName))
                {
                throw new ArgumentNullException($"{userName} is null or empty");
                }

            UserInfoExtension userInfo = null;
            try
                {
                userInfo = UserIdentityService.GetUserByPortalId(userName) as UserInfoExtension;
                }
            catch(Exception exception)
                {
                LoggerService.Log(exception);
                }

            if(userInfo == null)
                {
                throw new InvalidOperationException($"User {userName} is not a valid DoE user");
                }

            userInfo.Email = userInfo.Email.ToLower();
            userInfo.UserName = userInfo.UserName.ToLower();

            return userInfo;
            }
        public UserInfoExtension GetUserByEmail(string email)
            {
            email = email.Trim().ToLower();
            if(string.IsNullOrWhiteSpace(email))
                {
                throw new ArgumentNullException($"{email} is null or empty");
                }

            UserInfoExtension userInfo = null;
            try
                {
                userInfo = UserIdentityService.GetUserByEmail(email) as UserInfoExtension;
                }
            catch(Exception exception)
                {
                LoggerService.Log(exception);
                }

            if(userInfo == null)
                {
                throw new InvalidOperationException($"User {email} is not a valid DoE user");
                }

            userInfo.Email = userInfo.Email.ToLower();
            userInfo.UserName = userInfo.UserName.ToLower();

            return userInfo;
            }
        public UserInfoExtension GetUserAndCacheIt(string userName)
            {
            var uExt = GetUser(userName);

            uExt.InitialiseRoles(this.RepositoryFactory);
            

            SessionService.AddToSession(Cnt.CurrentUserKey, uExt);

            return uExt;
            }
        public UserInfoExtension GetUserAndCacheItInDb(string userName)
            {
            var uExt = GetUser(userName);

            // Get the User roles (from database) if the roleInitialised==false
            // load the roles
            // mark as initialised

            if(!uExt.IsRoleInitialised)
                {

                var sysUser = SysUserRepository.GetSysUserAndSaveItToDb(uExt.UserName);

                uExt.ActiveRoleOrgLevelList = new List<UserRoleModel>();
                uExt = GetUserAndCacheIt(sysUser.UserId);
                }

            SessionService.AddToSession(Cnt.CurrentUserKey, uExt);

            return uExt;

            }

        public bool ValidDoEUser(string userName)
            {
            try
                {
                var userInfo = UserIdentityService.GetUserByPortalId(userName);
                return userInfo.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase);
                }
            catch
                {
                return false;
                }

            }


        public
            UserInfoExtension GetCachedUserInfo()
            {
            return SessionService.ReadFromSession<UserInfoExtension>(Cnt.CurrentUserKey);
            }


        [Unity.Attributes.Dependency]
        public ILoggerService LoggerService { get; set; }

        [Unity.Attributes.Dependency]
        public IRepositoryFactory RepositoryFactory { get; set; }

        [Unity.Attributes.Dependency]
        public ISessionService SessionService { get; set; }

        [Unity.Attributes.Dependency]
        public IUserIdentityService UserIdentityService { get; set; }
        [Unity.Attributes.Dependency]
        public SysUserRepository SysUserRepository { get; set; }

        [Unity.Attributes.Dependency]
        public SysUserRoleRepository SysUserRoleRepository { get; set; }
        public void Dispose()
            {
            }
        }
    }

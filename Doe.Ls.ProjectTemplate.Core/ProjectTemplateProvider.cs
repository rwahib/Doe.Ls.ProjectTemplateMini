using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Security;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;


namespace Doe.Ls.ProjectTemplate.Core
{
    public class ProjectTemplateProvider : RoleProvider
    {

        public ProjectTemplateProvider():base()
        {
         
        }
        /// <summary>
        /// for unit test injections
        /// </summary>
        /// <param name="factory"></param>
        public ProjectTemplateProvider(IRepositoryFactory factory) : base()
        {
            _repository=new ServiceRepository(factory);
        }

        #region Overrides of RoleProvider

        public override string ApplicationName { get; set; } = "Position establishment";


        public override bool IsUserInRole(string username, string roleName)
        {
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(roleName)) return false;

            return GetRolesForUser(username).Any(userRole => string.Equals(userRole, roleName, StringComparison.CurrentCultureIgnoreCase));
        }


        public override string[] GetRolesForUser(string username)
        {

            var roles = new List<string>();

            if(string.IsNullOrEmpty(username)) return null;

            var loginSrv = ServiceRepository.LoginService();
            var userExt = loginSrv.GetCachedUserInfo();
            if (userExt!=null && userExt.UserName != username)
            {
                userExt = null;
            }
           
            // Get the User roles (from database) if the roleInitialised==false
            // load the roles
            // mark as initialised

            if(userExt == null || !userExt.IsRoleInitialised)
            {

                var sysUser = ServiceRepository.SysUserRepository().GetSysUserByUserName(username);
                userExt = UserInfoExtension.MapSysUser(sysUser, ServiceRepository.RepositoryFactory);
            }

            foreach(var sysUserRole in userExt.ActiveRoleOrgLevelList)
                {
                switch((Enums.UserRole)sysUserRole.RoleId)
                    {
                    case Enums.UserRole.SystemAdministrator:
                        roles.Add(Enums.UserRoleValues.SystemAdministrator);
                        break;
                    case Enums.UserRole.Administrator:
                        roles.Add(Enums.UserRoleValues.Administrator);
                        break;
                    case Enums.UserRole.PowerUser:
                        roles.Add(Enums.UserRoleValues.PowerUser);
                        break;
                    case Enums.UserRole.DivisionApprover:
                        roles.Add(Enums.UserRoleValues.DivisionApprover);
                        break;
                    case Enums.UserRole.DivisionEditor:
                        roles.Add(Enums.UserRoleValues.DivisionEditor);
                        break;

                    case Enums.UserRole.DirectorateEndorser:
                        roles.Add(Enums.UserRoleValues.DirectorateEndorser);
                        break;
                     case Enums.UserRole.DirectorateDataEntry:
                        roles.Add(Enums.UserRoleValues.DirectorateDataEntry);
                            break;
                    case Enums.UserRole.BusinessUnitAuthor:
                        roles.Add(Enums.UserRoleValues.BusinessUnitAuthor);
                        break;
                    case Enums.UserRole.BusinessUnitDataEntry:
                        roles.Add(Enums.UserRoleValues.BusinessUnitDataEntry);
                            break;
                    case Enums.UserRole.DoEUser:
                        roles.Add(Enums.UserRoleValues.DoEUser);
                        break;
                    case Enums.UserRole.Guest:
                        roles.Add(Enums.UserRoleValues.Guest);
                        
                        break;
                    }
                }

            return roles.ToArray();

            }
        
        public override void CreateRole(string roleName)
        {
            throw new NotSupportedException("this action is not supported 'CreateRole'");
            }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotSupportedException("this action is not supported 'DeleteRole'");

            }

        public override bool RoleExists(string roleName)
        {
            return ServiceRepository.SysRoleRepository().List().Any(r => r.RoleApiName.Equals(roleName, StringComparison.CurrentCulture));
            }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException("this action is not supported 'AddUsersToRoles'");
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException("this action is not supported 'RemoveUsersFromRoles'");

        }

        public override string[] GetUsersInRole(string roleName)
        {
            return ServiceRepository
                .SysUserRepository()
                .List(
                    )
                .Where(user => user.SysUserRoles.Any(
                            sysRole =>
                                sysRole.SysRole.RoleApiName.Equals(roleName, StringComparison.CurrentCultureIgnoreCase))).Select(user => user.UserId).ToArray();

            }

        public override string[] GetAllRoles()
        {
            return ServiceRepository.SysRoleRepository().List().Select(r => r.RoleApiName).ToArray();

            }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="usernameToMatch">part of username</param>
        /// <returns></returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return GetUsersInRole(roleName).Where(user => user.Contains(usernameToMatch)).ToArray();
        }

        #endregion

        private ServiceRepository _repository;

        public ServiceRepository ServiceRepository => _repository ?? (_repository = InitialiseRepository());

        private ServiceRepository InitialiseRepository()
        {
            var factory = new HttpRepositoryFactory();
            factory.RegisterAllDependencies();

            return new ServiceRepository(factory);
        }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Data;


namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories
    {
    public partial class SysUserRepository : BaseRepository<SysUser>
        {
        public SysUserRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService)
            : base(unitOfWork, loggerService, sessionService)
            {
            }

        public override IQueryable<SysUser> List()
            {
            return base.List()
                .Include(ent => ent.SysUserRoles)
                .Include(ent => ent.SysUserRoles.Select(sr => sr.SysRole))
                .Include(ent => ent.SysUserRoles.Select(sr => sr.OrgLevel))
                .OrderBy(ent => ent.UserId);
            }

        public IQueryable<SysUser> List(Enums.UserRole role)
            {
            var roleId = role.ToInteger();
            return base.List()
                .Include(ent => ent.SysUserRoles)
                .Include(ent => ent.SysUserRoles.Select(sr => sr.SysRole))
                .Include(ent => ent.SysUserRoles.Select(sr => sr.OrgLevel))
                .Where(ent => ent.SysUserRoles.Any(ur => ur.RoleId == roleId))
                .OrderBy(ent => ent.UserId);
            }

        public override void Insert(SysUser entity)
            {
            ValidateParametersAndSetDefaults(entity);

            if(ValidateEntity(entity).Count > 0)
                {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
                }

            base.Insert(entity);

            if(ValidateEntity(entity).Count > 0)
                {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
                }

            }

        public override void Update(SysUser entity, bool refresh = true)
            {
            entity.LastModifiedDate = DateTime.Now;
            if(entity.CreatedDate == DateTime.MinValue)
                {
                entity.CreatedDate = DateTime.Now;
                }
            if(ValidateEntity(entity).Count > 0)
                {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
                }

            base.Update(entity, refresh);
            }

        public IQueryable<SysUser> FilterSysUsers(IQueryable<SysUser> sysUsers,JQueryDataTableParamModel model)
        {
            var searchWord = model.sSearch;
            var filteredSysUser = sysUsers;
            if (!string.IsNullOrWhiteSpace(searchWord))
            {
                searchWord = searchWord.ToLower();
                filteredSysUser= filteredSysUser.Where(ent =>
                (!string.IsNullOrEmpty(ent.UserId) && ent.UserId.ToLower().Contains(searchWord))
                || (!string.IsNullOrEmpty(ent.Email) && ent.Email.ToLower().Contains(searchWord))                
                );
                }
            
            return filteredSysUser;
        }

        public SysUser GetSysUserByUserName(string simpleDecUserName)
            {
            return List().FirstOrDefault(su => su.UserId == simpleDecUserName);
            }

        public IQueryable<SysUser> GetSysUsersByRole(Enums.UserRole userRole, bool activeOnly = false)
            {

            return GetSysUsersByRole(userRole.ToInteger(), activeOnly);
            }

        public IQueryable<SysUser> GetSysUsersByRole(int roleId, bool activeOnly = false)
            {
            var usersInRole = List().Where(su => su.SysUserRoles.Any(r => r.RoleId == roleId));
            if(!activeOnly) return usersInRole;


            return
                usersInRole.Where(
                    su =>
                        su.SysUserRoles.Any(
                            r =>
                                r.RoleId == roleId & r.ActiveFrom <= DateTime.Now &&
                                (r.ActiveTo == null || r.ActiveTo >= DateTime.Now)));
            }

        private void ValidateParametersAndSetDefaults(SysUser entity)
            {

            if(entity.UserId.IsNullOrWhiteSpaceOrNa())
                throw new ValidationException($"{nameof(entity.UserId)} is required");
            entity.UserId = entity.UserId.ToLower();
            entity.CreatedDate = DateTime.Now;
            entity.LastModifiedDate = DateTime.Now;
            if(string.IsNullOrWhiteSpace(entity.CreatedBy))
                {
                entity.CreatedBy = SessionService.GetCurrentUser().UserName;
                }
            if(string.IsNullOrWhiteSpace(entity.LastModifiedBy))
                {
                entity.LastModifiedBy = SessionService.GetCurrentUser().UserName;
                }



            }

        public void Save(SysUser sysUser)
            {
            var dbSysUser = GetSysUserByUserName(sysUser.UserId);
            if(dbSysUser != null)
                {

                SetPropertyValuesFrom(ref dbSysUser, sysUser, true, new[] { "CreatedBy,CreatedDate" });

                Update(dbSysUser);
                }
            else
                {
                Insert(sysUser);
                }


            }

        public void Save(SysUser sysUser, SysUserRole sysUserRole)
            {
            Save(sysUser);
            var dbSysUserRole = this.SysUserRoleRepository.GetSysUserRoleById(sysUser.UserId, sysUserRole.RoleId, sysUserRole.OrgLevelId, sysUserRole.StructureId);
            if(dbSysUserRole != null)
                {

                this.SysUserRoleRepository.SetPropertyValuesFrom(ref dbSysUserRole, sysUserRole, true, new[] { "CreatedBy,CreatedDate" });

                this.SysUserRoleRepository.Update(dbSysUserRole);
                }
            else
                {
                this.SysUserRoleRepository.Insert(sysUserRole);
                }

            }

        [Unity.Attributes.Dependency]
        public SysUserRoleRepository SysUserRoleRepository { get; set; }

        [Unity.Attributes.Dependency]
        public SysRoleRepository SysRoleRepository { get; set; }

        public void SetSampleUser(ViewDataDictionary viewData)
            {
            var doeRoleId = Enums.UserRole.DoEUser.ToInteger();
            var doeId = Enums.UserRole.DoEUser.ToInteger();

            var doEUsers = this.List().Where(u => u.SysUserRoles.Count == 0 || u.SysUserRoles.All(su => su.RoleId == doeId));

            viewData[Enums.UserRole.DoEUser.ToString()] = doEUsers.Take(10).Select(u => u.UserId).ToArray();

            foreach(var sysRole in SysRoleRepository.List().Where(r => r.RoleId != doeRoleId).OrderBy(r => r.RoleId).ToArray())
                {

                viewData[sysRole.GetEnum().ToString()] = GetSysUsersByRole(sysRole.GetEnum(), true).Take(10).Select(u => u.UserId).ToArray();

                }


            }

        public SysUser GetSysUserByEmail(string email)
        {
            return List().SingleOrDefault(su => su.Email.ToLower() == email.ToLower());
        }
        public SysUser GetSysUserByEmailAndSaveItToDb(string email)
        {
            var srv=new ServiceRepository(this.RepositoryFactory);
            var sysUser = GetSysUserByEmail(email);
            if (sysUser != null) return sysUser;
            else
            {
                var userInfo = srv.LoginService().GetUserByEmail(email);
                sysUser = GetSysUserByEmail(userInfo.Email);
                if (sysUser != null) return sysUser;
                sysUser = userInfo.ToSysUser();
                sysUser.Active = true;
                sysUser.LastModifiedBy = sysUser.CreatedBy = Enums.Cnt.System;
                sysUser.LastModifiedDate = sysUser.CreatedDate = DateTime.Now;
                Insert(sysUser);
                Refresh(sysUser);
                return sysUser;
                }
        }
        public SysUser GetSysUserAndSaveItToDb(string userName)
            {
            var srv = new ServiceRepository(this.RepositoryFactory);
            var sysUser = GetSysUserByUserName(userName);
            if(sysUser != null) return sysUser;
            else
                {
                var userInfo = srv.LoginService().GetUser(userName);
                sysUser = userInfo.ToSysUser();
                sysUser.Active = true;
                sysUser.LastModifiedBy = sysUser.CreatedBy = Enums.Cnt.System;
                sysUser.LastModifiedDate = sysUser.CreatedDate = DateTime.Now;
                Insert(sysUser);
                return GetSysUserByUserName(userName);
                }
            }
        }
    }




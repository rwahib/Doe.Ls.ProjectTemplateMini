

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Doe.Ls.EntityBase;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.MVCExtensions;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Data;

using Unity.Attributes;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories
    {
    public partial class SysUserRoleRepository : BaseRepository<SysUserRole>
        {
        public SysUserRoleRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
            {
            }

        public override IQueryable<SysUserRole> List()
            {
            return base.List()
                    .Include(ent => ent.OrgLevel)
                    .Include(ent => ent.SysRole)
                    .Include(ent => ent.SysUser)
                    .OrderBy(ent => ent.UserId);
            }

        public void Grant(string userName, Enums.UserRole role, Enums.OrgLevel orgLevel,
            string structureId = "-1")
            {
            var result = GetSysUserRoleById(userName, role, orgLevel, structureId);
            if(result == null)
                {
                var sysUserRole = new SysUserRole
                    {
                    UserId = userName,
                    OrgLevelId = orgLevel.ToInteger(),
                    RoleId = role.ToInteger(),
                    StructureId = structureId,
                    OrgLevel = OrgLevelRepository.GetOrgLevelById(orgLevel.ToInteger())
                    };

                ValidateParametersAndSetDefaults(sysUserRole);

                base.Insert(sysUserRole);
                }
            }
        public void Grant(string userName, Enums.UserRole role, Enums.OrgLevel orgLevel,
            DateTime activeFrom,DateTime? activeTo=null, string structureId = "-1", string note="")
            {
            var result = GetSysUserRoleById(userName, role, orgLevel, structureId);
            if(result == null)
                {
                var sysUserRole = new SysUserRole
                    {
                    UserId = userName,
                    OrgLevelId = orgLevel.ToInteger(),
                    RoleId = role.ToInteger(),
                    StructureId = structureId,
                    ActiveFrom = activeFrom,
                    ActiveTo = activeTo,
                    Note = note,
                    OrgLevel = OrgLevelRepository.GetOrgLevelById(orgLevel.ToInteger())
                    };

                ValidateParametersAndSetDefaults(sysUserRole);

                base.Insert(sysUserRole);
                }
            }
        public void Grant(UserRoleModel userRoleModel)
        {
            Grant(userRoleModel.UserId, userRoleModel.UserRole, userRoleModel.OrgLevel,
            userRoleModel.ActiveFrom, userRoleModel.ActiveTo, userRoleModel.StructureId, userRoleModel.Note);
        }

        public void Deny(string userName, Enums.UserRole role, Enums.OrgLevel orgLevel,
            string structureId = "-1")
            {
            var result = GetSysUserRoleById(userName, role, orgLevel, structureId);
            if(result != null)
                {
                this.Delete(result);

                }
            }
        public override void Insert(SysUserRole entity)
            {
            ValidateParametersAndSetDefaults(entity);
            if(ValidateEntity(entity).Count > 0)
                {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
                }

            base.Insert(entity);
            }


        public SysUserRole GetSysUserRoleById(string userName, int roleId, int orgLevelId, string structureId = "-1")
        {
            var role = (Enums.UserRole) roleId;
            switch (role)
            {
                    case Enums.UserRole.SystemAdministrator:
                    case Enums.UserRole.Administrator:
                    case Enums.UserRole.PowerUser:
                case Enums.UserRole.HRDataEntry:
                    return List()
                        .SingleOrDefault(
                            sr =>
                                sr.RoleId == roleId && sr.UserId.ToLower() == userName.ToLower());
                default:
                    return
            List()
                .SingleOrDefault(
                    sr =>
                        sr.RoleId == roleId && sr.UserId.ToLower() == userName.ToLower() &&
                        sr.StructureId == structureId && sr.OrgLevelId == orgLevelId);
                }

            }
        

        public SysUserRole GetSysUserRoleById(string userName, Enums.UserRole role, Enums.OrgLevel orgLevel = Enums.OrgLevel.NA, string structureId = "-1")
            {
            var roleId = role.ToInteger();
            var orgLevelId = orgLevel.ToInteger();
            return
                GetSysUserRoleById(userName, roleId, orgLevelId, structureId);
            }
        public SysUserRole GetSysUserRoleByModel(UserRoleModel model)
        {
            return GetSysUserRoleById(model.UserId, model.RoleId, model.OrgLevelId, model.StructureId);
        }
        public override void Update(SysUserRole entity, bool refresh = true)
            {
            ValidateParametersAndSetDefaults(entity);

            if(ValidateEntity(entity).Count > 0)
                {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
                }

            base.Update(entity, refresh);
            }


        public void UpdateWithNewStructure(UserRoleModel model)
        {
            // separate the context
            var srv = new ServiceRepository(this.RepositoryFactory);
            var rep = srv.SysUserRoleRepository();
            var sysUserList = rep.List().Where(ur => ur.UserId == model.UserId && ur.RoleId == model.RoleId).ToList();
            if (sysUserList.Count == 0)
            {
                rep.Grant(model);
            }

           else if(sysUserList.Count == 1)
                {
                var sysUser = sysUserList.Single();
                var newModel = sysUser.ToUserOrgLevelObject(srv);

                rep .Delete(sysUser);

                newModel.StructureId = model.StructureId;
                newModel.ActiveFrom = model.ActiveFrom;
                newModel.ActiveTo = model.ActiveTo;
                sysUser.Note = model.Note;
                newModel.UpdateSignature(this.RepositoryFactory);
                

                rep.Grant(newModel);
                }

            else throw new InvalidOperationException("Could not update roles  with users that have multiple roles. Please delete the role first then Grant new role");

            }




        public IQueryable<SysUserRole> FilterSysUserRoles(IQueryable<SysUserRole> sysUserRoles, SearchArg searchArg)
            {
            var searchWord = searchArg.Search.ToLower();
            var filteredSysUserRole = sysUserRoles.Where(ent =>
                    (!string.IsNullOrEmpty(ent.SysUser.FirstName) && ent.SysUser.FirstName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.SysRole.RoleApiName) && ent.SysRole.RoleApiName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.OrgLevel.OrgLevelTitle) && ent.OrgLevel.OrgLevelTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredSysUserRole.OrderBy(e => e.UserId);
            }
        
        public List<UserRoleModel> GetSysUserRoleList(Enums.UserRole role)
            {
            var srv = new ServiceRepository(this.RepositoryFactory);
            var roleId = role.ToInteger();
            var list = List().Where(sr => sr.RoleId == roleId).AsNoTracking();
            var result = new List<UserRoleModel>();
            foreach(var sysUserRole in list)
                {
                result.Add(sysUserRole.ToUserOrgLevelObject(srv));
                }

            return result;
            }

        public void LogGrantSecurityActivity(UserRoleModel model, string modifiedBy, Enums.UserRole modifiedByUserRole, string context = "")
            {

            var logItem = new GeneralLog
                {
                Action = Enums.LogActions.ModifiyUserRole.GetDescription(),
                Context = $"the user {model.UserId} has been granted for {model.RoleName} role by {modifiedBy}-{modifiedByUserRole.GetDescription()}",
                Username = modifiedBy,
                RoleId = modifiedByUserRole.ToInteger(),
                Note = $"Active from {model.ActiveFrom}  Active To {model.ActiveTo} other info :{context}",
                CreationDate = DateTime.Now,

                };
            this.GeneralLogger.Insert(logItem);            
            }
        public void LogDeleteSecurityActivity(UserRoleModel model, string modifiedBy, Enums.UserRole modifiedByUserRole, string context = "")
            {

            var logItem = new GeneralLog
                {
                Action = Enums.LogActions.ModifiyUserRole.GetDescription(),
                Context = $"the user {model.UserId} has been revoked for {model.RoleName} role by {modifiedBy}-{modifiedByUserRole.GetDescription()}",
                Username = modifiedBy,
                RoleId = modifiedByUserRole.ToInteger(),
                Note = $"Active from {model.ActiveFrom}  Active To {model.ActiveTo} other info :{context}",
                CreationDate = DateTime.Now,

                };
            this.GeneralLogger.Insert(logItem);
            }
        public void LogUpdateSecurityActivity(UserRoleModel model, string modifiedBy, Enums.UserRole modifiedByUserRole, string context = "")
            { 
            var logItem = new GeneralLog
                {
                Action = Enums.LogActions.ModifiyUserRole.GetDescription(),
                Context = $"the user {model.UserId} has been updated for {model.RoleName} role by {modifiedBy}-{modifiedByUserRole.GetDescription()}",
                Username = modifiedBy,
                RoleId = modifiedByUserRole.ToInteger(),
                Note = $"Active from {model.ActiveFrom}  Active To {model.ActiveTo} other info :{context}",
                CreationDate = DateTime.Now,

                };
            this.GeneralLogger.Insert(logItem);
            }

        private void ValidateParametersAndSetDefaults(SysUserRole entity)
            {

            if(entity.UserId.IsNullOrWhiteSpaceOrNa()) throw new ValidationException($"{nameof(entity.UserId)} is required");
            if(entity.RoleId == int.MinValue) throw new ValidationException($"{nameof(entity.RoleId)} is required");
            if(entity.OrgLevelId == int.MinValue) throw new ValidationException($"{nameof(entity.OrgLevelId)} is required");
            if(entity.OrgLevelId != Enums.OrgLevel.NA.ToInteger() && entity.OrgLevelId != Enums.OrgLevel.Application.ToInteger()
                )
                {
                if(entity.StructureId == "-1")
                    throw new ValidationException(
                        $"{nameof(entity.StructureId)} is required for organisational level {((Enums.OrgLevel)entity.OrgLevelId)}");
                }

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

            if(entity.ActiveFrom == DateTime.MinValue) entity.ActiveFrom = DateTime.Now.AddYears(-1);

            }

        [Unity.Attributes.Dependency]
        public OrgLevelRepository OrgLevelRepository { get; set; }
        [Unity.Attributes.Dependency]
        public GeneralLogRepository GeneralLogger { get; set; }


       
        }
    }




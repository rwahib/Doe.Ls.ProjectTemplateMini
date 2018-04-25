 


using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Data;

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
                    .Include(ent=>ent.OrgLevel) 
                    .Include(ent=>ent.SysRole) 
                    .Include(ent=>ent.SysUser) 
                    .OrderBy(ent=>ent.UserId);
        }

        public override void Insert(SysUserRole entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(SysUserRole entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<SysUserRole> FilterSysUserRoles(IQueryable<SysUserRole> sysUserRoles, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredSysUserRole = sysUserRoles.Where(ent => 
                    (!string.IsNullOrEmpty(ent.SysUser.FirstName) && ent.SysUser.FirstName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.SysRole.RoleApiName) && ent.SysRole.RoleApiName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.StructureId) && ent.StructureId.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.OrgLevel.OrgLevelName) && ent.OrgLevel.OrgLevelName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Note) && ent.Note.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredSysUserRole.OrderBy(e => e.UserId);
        }
    }
}




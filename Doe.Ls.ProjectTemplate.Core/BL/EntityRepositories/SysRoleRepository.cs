

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
    public partial class SysRoleRepository : BaseRepository<SysRole> 
    {
        public SysRoleRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<SysRole> List()
        {                       
            return base.List()
                    .Include(ent=>ent.SysUserRoles) 
                    .OrderBy(ent=>ent.RoleId);
        }

        public override void Insert(SysRole entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(SysRole entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<SysRole> FilterSysRoles(IQueryable<SysRole> sysRoles, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredSysRole = sysRoles.Where(ent => 
                    ent.RoleId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.RoleTitle) && ent.RoleTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.RoleApiName) && ent.RoleApiName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.RoleDescription) && ent.RoleDescription.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredSysRole.OrderBy(e => e.RoleId);
        }

        public SysRole GetSysRoleById(int roleId)
        {
            return List().SingleOrDefault(sr => sr.RoleId == roleId);
        }
    }
}




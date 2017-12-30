

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
    public partial class SysUserRepository : BaseRepository<SysUser> 
    {
        public SysUserRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<SysUser> List()
        {                       
            return base.List()
                    .Include(ent=>ent.SysUserRoles) 
                    .OrderBy(ent=>ent.UserId);
        }

        public override void Insert(SysUser entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(SysUser entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<SysUser> FilterSysUsers(IQueryable<SysUser> sysUsers, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredSysUser = sysUsers.Where(ent => 
                    (!string.IsNullOrEmpty(ent.UserId) && ent.UserId.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.FirstName) && ent.FirstName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.LastName) && ent.LastName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Email) && ent.Email.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.PrimaryPhone) && ent.PrimaryPhone.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Note) && ent.Note.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredSysUser.OrderBy(e => e.UserId);
        }
    }
}




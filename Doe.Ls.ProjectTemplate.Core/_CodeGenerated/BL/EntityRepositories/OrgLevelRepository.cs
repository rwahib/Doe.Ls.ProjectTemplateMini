 


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
    public partial class OrgLevelRepository : BaseRepository<OrgLevel> 
    {
        public OrgLevelRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<OrgLevel> List()
        {                       
            return base.List()
                    .Include(ent=>ent.SysUserRoles) 
                    .OrderBy(ent=>ent.OrgLevelId);
        }

        public override void Insert(OrgLevel entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(OrgLevel entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<OrgLevel> FilterOrgLevels(IQueryable<OrgLevel> orgLevels, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredOrgLevel = orgLevels.Where(ent => 
                    ent.OrgLevelId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.OrgLevelTitle) && ent.OrgLevelTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.OrgLevelName) && ent.OrgLevelName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Description) && ent.Description.ToLower().Contains(searchWord))
);

            return filteredOrgLevel.OrderBy(e => e.OrgLevelId);
        }
    }
}




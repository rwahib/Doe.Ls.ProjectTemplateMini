

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
    public partial class StatusValueRepository : BaseRepository<StatusValue> 
    {
        public StatusValueRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<StatusValue> List()
        {                       
            return base.List()
                    .Include(ent=>ent.BusinessUnits) 
                    .Include(ent=>ent.Directorates) 
                    .Include(ent=>ent.Executives) 
                    .Include(ent=>ent.FunctionalAreas) 
                    .Include(ent=>ent.Grades) 
                    .Include(ent=>ent.RolePositionDescriptions) 
                    .Include(ent=>ent.Units) 
                    .Include(ent=>ent.Positions) 
                    .OrderBy(ent=>ent.StatusId);
        }

        public override void Insert(StatusValue entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(StatusValue entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<StatusValue> FilterStatusValues(IQueryable<StatusValue> statusValues, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredStatusValue = statusValues.Where(ent => 
                    (!string.IsNullOrEmpty(ent.StatusName) && ent.StatusName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.StatusDescription) && ent.StatusDescription.ToLower().Contains(searchWord))
);

            return filteredStatusValue.OrderBy(e => e.StatusId);
        }
    }
}




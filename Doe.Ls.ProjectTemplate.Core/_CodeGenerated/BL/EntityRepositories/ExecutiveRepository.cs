 


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
    public partial class ExecutiveRepository : BaseRepository<Executive> 
    {
        public ExecutiveRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<Executive> List()
        {                       
            return base.List()
                    .Include(ent=>ent.Directorates) 
                    .Include(ent=>ent.StatusValue) 
                    .OrderBy(ent=>ent.ExecutiveCod);
        }

        public override void Insert(Executive entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(Executive entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<Executive> FilterExecutives(IQueryable<Executive> executives, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredExecutive = executives.Where(ent => 
                    (!string.IsNullOrEmpty(ent.ExecutiveCod) && ent.ExecutiveCod.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.ExecutiveTitle) && ent.ExecutiveTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.ExecutiveDescription) && ent.ExecutiveDescription.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CustomClass) && ent.CustomClass.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.DefaultExecutiveOverview) && ent.DefaultExecutiveOverview.ToLower().Contains(searchWord))
);

            return filteredExecutive.OrderBy(e => e.ExecutiveCod);
        }
    }
}




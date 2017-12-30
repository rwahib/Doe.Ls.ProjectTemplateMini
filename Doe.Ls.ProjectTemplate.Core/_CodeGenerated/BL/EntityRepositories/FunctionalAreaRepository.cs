

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
    public partial class FunctionalAreaRepository : BaseRepository<FunctionalArea> 
    {
        public FunctionalAreaRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<FunctionalArea> List()
        {                       
            return base.List()
                    .Include(ent=>ent.Directorate) 
                    .Include(ent=>ent.StatusValue) 
                    .Include(ent=>ent.Units) 
                    .OrderBy(ent=>ent.FuncationalAreaId);
        }

        public override void Insert(FunctionalArea entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(FunctionalArea entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<FunctionalArea> FilterFunctionalAreas(IQueryable<FunctionalArea> functionalAreas, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredFunctionalArea = functionalAreas.Where(ent => 
                    ent.FuncationalAreaId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.AreanName) && ent.AreanName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.AreaDescription) && ent.AreaDescription.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Directorate.DirectorateName) && ent.Directorate.DirectorateName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.AreaCustomClass) && ent.AreaCustomClass.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredFunctionalArea.OrderBy(e => e.FuncationalAreaId);
        }
    }
}




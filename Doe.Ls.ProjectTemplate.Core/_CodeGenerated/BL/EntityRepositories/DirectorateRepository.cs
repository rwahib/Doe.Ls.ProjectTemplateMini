 


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
    public partial class DirectorateRepository : BaseRepository<Directorate> 
    {
        public DirectorateRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<Directorate> List()
        {                       
            return base.List()
                    .Include(ent=>ent.BusinessUnits) 
                    .Include(ent=>ent.Executive) 
                    .Include(ent=>ent.StatusValue) 
                    .Include(ent=>ent.FunctionalAreas) 
                    .Include(ent=>ent.Locations) 
                    .OrderBy(ent=>ent.DirectorateId);
        }

        public override void Insert(Directorate entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(Directorate entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<Directorate> FilterDirectorates(IQueryable<Directorate> directorates, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredDirectorate = directorates.Where(ent => 
                    ent.DirectorateId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Executive.ExecutiveTitle) && ent.Executive.ExecutiveTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.DirectorateName) && ent.DirectorateName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.DirectorateDescription) && ent.DirectorateDescription.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.DirectorateOverview) && ent.DirectorateOverview.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.DirectorateCustomClass) && ent.DirectorateCustomClass.ToLower().Contains(searchWord))
                    || ent.DirectorateOrder.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredDirectorate.OrderBy(e => e.DirectorateId);
        }
    }
}




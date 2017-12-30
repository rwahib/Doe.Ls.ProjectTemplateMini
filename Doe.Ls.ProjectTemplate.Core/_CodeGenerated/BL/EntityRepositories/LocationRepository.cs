

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
    public partial class LocationRepository : BaseRepository<Location> 
    {
        public LocationRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<Location> List()
        {                       
            return base.List()
                    .Include(ent=>ent.Directorate) 
                    .Include(ent=>ent.Positions) 
                    .OrderBy(ent=>ent.LocationId);
        }

        public override void Insert(Location entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(Location entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<Location> FilterLocations(IQueryable<Location> locations, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredLocation = locations.Where(ent => 
                    ent.LocationId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Name) && ent.Name.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Directorate.DirectorateName) && ent.Directorate.DirectorateName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredLocation.OrderBy(e => e.LocationId);
        }
    }
}






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
using Doe.Ls.ProjectTemplate.Core.BL.Models;
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
            var na = Enums.Cnt.Na;
            return base.List().Where(en => en.LocationId != na)
                    .Include(ent => ent.Directorate)
                    .Include(ent => ent.Directorate.Executive)
                    .OrderBy(ent => ent.LocationId);
        }

        public override void Insert(Location entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.LastModifiedDate = DateTime.Now;
            
            var id = this.GetDbNewId("Location");
            entity.LocationId = id;

            if (ValidateEntity(entity).Count > 0)
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }

        public override void Update(Location entity, bool refresh = true)
        {
            entity.LastModifiedDate = DateTime.Now;
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
                    || (!string.IsNullOrEmpty(ent.LastModifiedBy) && ent.LastModifiedBy.ToLower().Contains(searchWord))
);

            return filteredLocation.OrderBy(e => e.LocationId);
        }

        public IQueryable<Location> GetLocationsByUnit(int unitId)
        {
            return List().Where(l => l.Directorate.BusinessUnits.Any(bu=>bu.Units.Any(u=>u.UnitId==unitId)));
        }

        public Location GetLocationById(int locationId)
        {
            return List().SingleOrDefault(l => l.LocationId == locationId);
        }

        public Location GetLocationByIdWithPositions(int locationId)
        {
            return List().Include(l => l.Positions).SingleOrDefault(l => l.LocationId == locationId);
        }


        public IQueryable<Location> BaseList()
        {
            var na = Enums.Cnt.Na;
            return base.List().Where(en => en.LocationId != na)
                    .Include(ent => ent.Directorate)
                    .OrderBy(ent => ent.Name);
        }

        public IEnumerable<Location> DistinctList()
        {
            return BaseList().ToList().GroupBy(l => l.Name).Select(g => g.First()).ToList();
        }

        public IEnumerable<Location> LocationListByDirectorate(int directorateId)
        {
            return BaseList().Where(l => l.DirectorateId == directorateId).ToList();
        }

        

    }
}




using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
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
            var na = Enums.Cnt.Na;
            return base.List().Where(d=>d.DirectorateId!=na)
                    .Include(ent=>ent.Executive) 
                    .Include(ent=>ent.StatusValue) 
                    .Include(ent=>ent.FunctionalAreas) 
                    .Include(ent=>ent.Locations)
                    .Include(ent=>ent.BusinessUnits.Select(bu=>bu.Units))                     
                    .OrderBy(ent=>ent.DirectorateId);
        }
        public IQueryable<Directorate> BasicList()
        {
            var na = Enums.Cnt.Na;
            return base.List().Where(d => d.ExecutiveCod != na.ToString());

        }

        public IQueryable<Directorate> ListWithLocations()
        {
            var na = Enums.Cnt.Na;
            return base.List().Where(d => d.ExecutiveCod != na.ToString())
                .Include(ent => ent.Locations);

        }

        public Directorate GetDirectorateById( int directorateId)
        {
            return List().SingleOrDefault(e => e.DirectorateId == directorateId);

        }
        public override void Insert(Directorate entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            entity.CreatedDate = DateTime.Now;
            entity.LastModifiedDate = DateTime.Now;
            if(!string.IsNullOrEmpty(entity.DirectorateOverview))
                entity.DirectorateOverview = entity.DirectorateOverview.Trim();

            base.Insert(entity);
        }
        
        public override void Update(Directorate entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }
            entity.LastModifiedDate = DateTime.Now;
            if (!string.IsNullOrEmpty(entity.DirectorateOverview))
                entity.DirectorateOverview = entity.DirectorateOverview.Trim();
            base.Update(entity, refresh);
        }

        public override void Delete(Directorate entity)
        {
            LoadNavigationProperty(entity,ent=>ent.Locations);
            if (entity.Locations.Count > 0) {
                LocationRepository.RemoveRange(entity.Locations.ToList());

            }

            base.Delete(entity);
        }

        public IQueryable<Directorate> FilterDirectorates(IQueryable<Directorate> directorates, BasicStructureArgument searchArg)
        {
            var searchWord = searchArg.sSearch;
            if (!string.IsNullOrWhiteSpace(searchArg.DivisionCode))
            {
                directorates = directorates.Where(d => d.ExecutiveCod == searchArg.DivisionCode);
            }
            var filteredDirectorate =string.IsNullOrWhiteSpace(searchWord)? directorates : directorates.Where(ent => 
                    ent.DirectorateId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Executive.ExecutiveTitle) && ent.Executive.ExecutiveTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.DirectorateName) && ent.DirectorateName.ToLower().Contains(searchWord))                    
                    || (!string.IsNullOrEmpty(ent.LastModifiedBy) && ent.LastModifiedBy.ToLower().Contains(searchWord))                    
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredDirectorate.OrderBy(e => e.DirectorateId);
        }

        [Unity.Attributes.Dependency]
        public LocationRepository LocationRepository { get; set; }

    }
}




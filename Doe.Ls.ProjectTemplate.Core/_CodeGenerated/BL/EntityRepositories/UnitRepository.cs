

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
    public partial class UnitRepository : BaseRepository<Unit> 
    {
        public UnitRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<Unit> List()
        {                       
            return base.List()
                    .Include(ent=>ent.BusinessUnit) 
                    .Include(ent=>ent.FunctionalArea) 
                    .Include(ent=>ent.HierarchyLevel) 
                    .Include(ent=>ent.StatusValue) 
                    .Include(ent=>ent.TeamType) 
                    .Include(ent=>ent.UnitList) 
                    .Include(ent=>ent.ParentUnit) 
                    .Include(ent=>ent.Positions) 
                    .OrderBy(ent=>ent.UnitId);
        }

        public override void Insert(Unit entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(Unit entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<Unit> FilterUnits(IQueryable<Unit> units, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredUnit = units.Where(ent => 
                    ent.UnitId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.UnitName) && ent.UnitName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.UnitDescription) && ent.UnitDescription.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.BusinessUnit.BUnitName) && ent.BusinessUnit.BUnitName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.FunctionalArea.AreanName) && ent.FunctionalArea.AreanName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Unit.UnitName) && ent.Unit.UnitName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.HierarchyLevel.HierarchyName) && ent.HierarchyLevel.HierarchyName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.TeamType.TeamTypeName) && ent.TeamType.TeamTypeName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.UnitCustomClass) && ent.UnitCustomClass.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredUnit.OrderBy(e => e.UnitId);
        }
    }
}




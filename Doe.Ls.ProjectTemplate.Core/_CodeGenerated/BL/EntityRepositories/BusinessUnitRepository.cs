

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
    public partial class BusinessUnitRepository : BaseRepository<BusinessUnit> 
    {
        public BusinessUnitRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<BusinessUnit> List()
        {                       
            return base.List()
                    .Include(ent=>ent.Directorate) 
                    .Include(ent=>ent.HierarchyLevel) 
                    .Include(ent=>ent.StatusValue) 
                    .Include(ent=>ent.Units) 
                    .OrderBy(ent=>ent.BUnitId);
        }

        public override void Insert(BusinessUnit entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(BusinessUnit entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<BusinessUnit> FilterBusinessUnits(IQueryable<BusinessUnit> businessUnits, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredBusinessUnit = businessUnits.Where(ent => 
                    ent.BUnitId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Directorate.DirectorateName) && ent.Directorate.DirectorateName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.HierarchyLevel.HierarchyName) && ent.HierarchyLevel.HierarchyName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.BUnitName) && ent.BUnitName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.BUnitDescription) && ent.BUnitDescription.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredBusinessUnit.OrderBy(e => e.BUnitId);
        }
    }
}




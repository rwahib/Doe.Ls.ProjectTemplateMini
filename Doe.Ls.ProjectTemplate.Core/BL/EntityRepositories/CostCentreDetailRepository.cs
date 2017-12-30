

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
    public partial class CostCentreDetailRepository : BaseRepository<CostCentreDetail> 
    {
        public CostCentreDetailRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<CostCentreDetail> List()
        {                       
            return base.List()
                    .Include(ent=>ent.Position) 
                    .OrderBy(ent=>ent.PositionId);
        }

        public override void Insert(CostCentreDetail entity) 
        {
            entity.LastUpdatedDate = DateTime.Now;
           
            if (string.IsNullOrWhiteSpace(entity.LastUpdatedBy))
            {
                entity.LastUpdatedBy = SessionService.GetCurrentUser().UserName;
            }
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(CostCentreDetail entity, bool refresh = true) 
        {
            entity.LastUpdatedDate = DateTime.Now;

            if (string.IsNullOrWhiteSpace(entity.LastUpdatedBy))
            {
                entity.LastUpdatedBy = SessionService.GetCurrentUser().UserName;
            }

            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<CostCentreDetail> FilterCostCentreDetails(IQueryable<CostCentreDetail> costCentreDetails, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredCostCentreDetail = costCentreDetails.Where(ent => 
                    (!string.IsNullOrEmpty(ent.Position.PositionTitle) && ent.Position.PositionTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CostCentre) && ent.CostCentre.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Fund) && ent.Fund.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.PayrollWBS) && ent.PayrollWBS.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.LastUpdatedBy) && ent.LastUpdatedBy.ToLower().Contains(searchWord))
);

            return filteredCostCentreDetail.OrderBy(e => e.PositionId);
        }
    }
}




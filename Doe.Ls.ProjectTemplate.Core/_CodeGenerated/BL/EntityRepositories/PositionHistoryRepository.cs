

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
    public partial class PositionHistoryRepository : BaseRepository<PositionHistory> 
    {
        public PositionHistoryRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<PositionHistory> List()
        {                       
            return base.List()
                    .Include(ent=>ent.Position) 
                    .OrderBy(ent=>ent.PositionHistoryId);
        }

        public override void Insert(PositionHistory entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(PositionHistory entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<PositionHistory> FilterPositionHistorys(IQueryable<PositionHistory> positionHistorys, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredPositionHistory = positionHistorys.Where(ent => 
                    ent.PositionHistoryId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Position.PositionTitle) && ent.Position.PositionTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Action) && ent.Action.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.StatusFrom) && ent.StatusFrom.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.StatusTo) && ent.StatusTo.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.AdditionalInfo) && ent.AdditionalInfo.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredPositionHistory.OrderBy(e => e.PositionHistoryId);
        }
    }
}




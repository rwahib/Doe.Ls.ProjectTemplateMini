 


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
    public partial class RolePositionDescriptionHistoryRepository : BaseRepository<RolePositionDescriptionHistory> 
    {
        public RolePositionDescriptionHistoryRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<RolePositionDescriptionHistory> List()
        {                       
            return base.List()
                    .Include(ent=>ent.RolePositionDescription) 
                    .OrderBy(ent=>ent.RolePositionDescriptionHistoryId);
        }

        public override void Insert(RolePositionDescriptionHistory entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(RolePositionDescriptionHistory entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<RolePositionDescriptionHistory> FilterRolePositionDescriptionHistorys(IQueryable<RolePositionDescriptionHistory> rolePositionDescriptionHistorys, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredRolePositionDescriptionHistory = rolePositionDescriptionHistorys.Where(ent => 
                    ent.RolePositionDescriptionHistoryId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.RolePositionDescription.Title) && ent.RolePositionDescription.Title.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Action) && ent.Action.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.StatusFrom) && ent.StatusFrom.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.StatusTo) && ent.StatusTo.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.AdditionalInfo) && ent.AdditionalInfo.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredRolePositionDescriptionHistory.OrderBy(e => e.RolePositionDescriptionHistoryId);
        }
    }
}




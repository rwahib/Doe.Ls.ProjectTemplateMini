

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
    public partial class CapabilityBehaviourIndicatorRepository : BaseRepository<CapabilityBehaviourIndicator> 
    {
        public CapabilityBehaviourIndicatorRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<CapabilityBehaviourIndicator> List()
        {                       
            return base.List()
                    .Include(ent=>ent.CapabilityLevel) 
                    .Include(ent=>ent.CapabilityName) 
                    .OrderBy(ent=>ent.CapabilityNameId);
        }

        public override void Insert(CapabilityBehaviourIndicator entity) 
        {
            entity.DateCreated = DateTime.Now;
            entity.LastUpdated = DateTime.Now;
            if (string.IsNullOrWhiteSpace(entity.CreatedBy))
            {
                entity.CreatedBy = SessionService.GetCurrentUser().UserName;
            }
            if (string.IsNullOrWhiteSpace(entity.LastModifiedBy))
            {
                entity.LastModifiedBy = SessionService.GetCurrentUser().UserName;
            }

            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(CapabilityBehaviourIndicator entity, bool refresh = true) 
        {
            entity.LastUpdated = DateTime.Now;
            if (string.IsNullOrWhiteSpace(entity.LastModifiedBy))
            {
                entity.LastModifiedBy = SessionService.GetCurrentUser().UserName;
            }


            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<CapabilityBehaviourIndicator> FilterCapabilityBehaviourIndicators(IQueryable<CapabilityBehaviourIndicator> capabilityBehaviourIndicators, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredCapabilityBehaviourIndicator = capabilityBehaviourIndicators.Where(ent => 
                    (!string.IsNullOrEmpty(ent.CapabilityName.Name) && ent.CapabilityName.Name.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CapabilityLevel.LevelName) && ent.CapabilityLevel.LevelName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.IndicatorContext) && ent.IndicatorContext.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredCapabilityBehaviourIndicator.OrderBy(e => e.CapabilityNameId);
        }

        public CapabilityBehaviourIndicator GetBehaviourIndicatorByNameAndLevel(int levelId,int nameId)
        {
           return this
                 .List()
                 .FirstOrDefault(cb => cb.CapabilityLevelId == levelId && cb.CapabilityNameId == nameId);
        }
    }
}




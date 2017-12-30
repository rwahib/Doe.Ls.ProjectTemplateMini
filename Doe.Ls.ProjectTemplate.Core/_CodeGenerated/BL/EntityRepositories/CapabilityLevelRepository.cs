

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
    public partial class CapabilityLevelRepository : BaseRepository<CapabilityLevel> 
    {
        public CapabilityLevelRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<CapabilityLevel> List()
        {                       
            return base.List()
                    .Include(ent=>ent.CapabilityBehaviourIndicators) 
                    .Include(ent=>ent.RoleCapabilities) 
                    .OrderBy(ent=>ent.CapabilityLevelId);
        }

        public override void Insert(CapabilityLevel entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(CapabilityLevel entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<CapabilityLevel> FilterCapabilityLevels(IQueryable<CapabilityLevel> capabilityLevels, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredCapabilityLevel = capabilityLevels.Where(ent => 
                    ent.CapabilityLevelId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.LevelName) && ent.LevelName.ToLower().Contains(searchWord))
                    || ent.DisplayOrder.ToString().Contains(searchWord)
                    || ent.LevelOrder.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredCapabilityLevel.OrderBy(e => e.CapabilityLevelId);
        }
    }
}




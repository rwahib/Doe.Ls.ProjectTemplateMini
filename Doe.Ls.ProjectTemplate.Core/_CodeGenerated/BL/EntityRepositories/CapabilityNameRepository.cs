

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
    public partial class CapabilityNameRepository : BaseRepository<CapabilityName> 
    {
        public CapabilityNameRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<CapabilityName> List()
        {                       
            return base.List()
                    .Include(ent=>ent.CapabilityBehaviourIndicators) 
                    .Include(ent=>ent.CapabilityGroup) 
                    .Include(ent=>ent.RoleCapabilities) 
                    .OrderBy(ent=>ent.CapabilityNameId);
        }

        public override void Insert(CapabilityName entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(CapabilityName entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<CapabilityName> FilterCapabilityNames(IQueryable<CapabilityName> capabilityNames, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredCapabilityName = capabilityNames.Where(ent => 
                    ent.CapabilityNameId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Name) && ent.Name.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CapabilityDescription) && ent.CapabilityDescription.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CapabilityGroup.GroupName) && ent.CapabilityGroup.GroupName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredCapabilityName.OrderBy(e => e.CapabilityNameId);
        }
    }
}




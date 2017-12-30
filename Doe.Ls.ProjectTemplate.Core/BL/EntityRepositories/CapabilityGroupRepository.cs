

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
    public partial class CapabilityGroupRepository : BaseRepository<CapabilityGroup> 
    {
        public CapabilityGroupRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<CapabilityGroup> List()
        {                       
            return base.List()
                    .Include(ent=>ent.CapabilityNames) 
                    .OrderBy(ent=>ent.DisplayOrder);
        }

        public override void Insert(CapabilityGroup entity) 
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
        
        public override void Update(CapabilityGroup entity, bool refresh = true) 
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

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<CapabilityGroup> FilterCapabilityGroups(IQueryable<CapabilityGroup> capabilityGroups, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredCapabilityGroup = capabilityGroups.Where(ent => 
                    ent.CapabilityGroupId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.GroupName) && ent.GroupName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.GroupDescription) && ent.GroupDescription.ToLower().Contains(searchWord))
                    || ent.DisplayOrder.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.GroupImage) && ent.GroupImage.ToLower().Contains(searchWord))
);

            return filteredCapabilityGroup.OrderBy(e => e.CapabilityGroupId);
        }
    }
}




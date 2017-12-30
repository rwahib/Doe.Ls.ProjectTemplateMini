

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
    public partial class RoleCapabilityRepository : BaseRepository<RoleCapability> 
    {
        public RoleCapabilityRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<RoleCapability> List()
        {                       
            return base.List()
                    .Include(ent=>ent.CapabilityLevel) 
                    .Include(ent=>ent.CapabilityName) 
                    .Include(ent=>ent.RoleDescription) 
                    .OrderBy(ent=>ent.RoleDescriptionId);
        }

        public override void Insert(RoleCapability entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(RoleCapability entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<RoleCapability> FilterRoleCapabilitys(IQueryable<RoleCapability> roleCapabilitys, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredRoleCapability = roleCapabilitys.Where(ent => 
                    (!string.IsNullOrEmpty(ent.RoleDescription.OldPDFileName) && ent.RoleDescription.OldPDFileName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CapabilityName.Name) && ent.CapabilityName.Name.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CapabilityLevel.LevelName) && ent.CapabilityLevel.LevelName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredRoleCapability.OrderBy(e => e.RoleDescriptionId);
        }
    }
}




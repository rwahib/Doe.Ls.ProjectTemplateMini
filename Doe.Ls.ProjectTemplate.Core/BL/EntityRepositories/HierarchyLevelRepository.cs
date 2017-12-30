

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
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories 
{
    public partial class HierarchyLevelRepository : BaseRepository<HierarchyLevel> 
    {
        public HierarchyLevelRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<HierarchyLevel> List()
        {
            var na = Enums.Cnt.Na;
            return base.List().Where(d => d.HierarchyId != na)
                    .Include(ent=>ent.BusinessUnits) 
                    .Include(ent=>ent.Units) 
                    .OrderBy(ent=>ent.HierarchyId);
        }

        public override void Insert(HierarchyLevel entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(HierarchyLevel entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<HierarchyLevel> FilterHierarchyLevels(IQueryable<HierarchyLevel> hierarchyLevels, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredHierarchyLevel = hierarchyLevels.Where(ent => 
                    ent.HierarchyId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.HierarchyName) && ent.HierarchyName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.HierarchyDescription) && ent.HierarchyDescription.ToLower().Contains(searchWord))
);

            return filteredHierarchyLevel.OrderBy(e => e.HierarchyId);
        }
    }
}




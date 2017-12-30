

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

using Unity.Attributes;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories 
{
    public partial class ExecutiveRepository : BaseRepository<Executive> 
    {
        public ExecutiveRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<Executive> List()
        {
            var na = Enums.Cnt.Na.ToString();
            return base.List().Where(ent => ent.ExecutiveCod!= na)
                     .Include(ent => ent.Directorates.Select(d => d.BusinessUnits.Select(bu => bu.Units)))
                    .Include(ent=>ent.StatusValue) 
                    .OrderBy(ent=>ent.ExecutiveCod);
        }
        public  IQueryable<Executive> AllList()
        {
            return base.List()                    
                                  
                    .Include(ent => ent.StatusValue)
                    .OrderBy(ent => ent.ExecutiveCod);
        }
        public Executive GetExecutiveByCode(string executiveCod)
            {
            return List().SingleOrDefault(e => e.ExecutiveCod == executiveCod);

            }
        public override void Insert(Executive entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }
            entity.ExecutiveCod = entity.ExecutiveCod.ToUpper();
            base.Insert(entity);
        }
        
        public override void Update(Executive entity, bool refresh = true)
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }
            entity.ExecutiveCod = entity.ExecutiveCod.ToUpper();
            base.Update(entity, refresh);
        }

        

        public IQueryable<Executive> FilterExecutives(IQueryable<Executive> divisions, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredExecutive = divisions.Where(ent => 
                    (!string.IsNullOrEmpty(ent.ExecutiveCod) && ent.ExecutiveCod.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.ExecutiveTitle) && ent.ExecutiveTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.ExecutiveDescription) && ent.ExecutiveDescription.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredExecutive.OrderBy(e => e.ExecutiveCod);
        }

        [Unity.Attributes.Dependency]
        public DirectorateRepository DirectorateRepository { get; set; }

    }
}




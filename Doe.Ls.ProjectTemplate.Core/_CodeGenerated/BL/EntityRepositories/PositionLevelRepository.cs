 


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
    public partial class PositionLevelRepository : BaseRepository<PositionLevel> 
    {
        public PositionLevelRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<PositionLevel> List()
        {                       
            return base.List()
                    .Include(ent=>ent.Positions) 
                    .OrderBy(ent=>ent.PositionLevelId);
        }

        public override void Insert(PositionLevel entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(PositionLevel entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<PositionLevel> FilterPositionLevels(IQueryable<PositionLevel> positionLevels, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredPositionLevel = positionLevels.Where(ent => 
                    ent.PositionLevelId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.PositionLevelName) && ent.PositionLevelName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CustomClass) && ent.CustomClass.ToLower().Contains(searchWord))
);

            return filteredPositionLevel.OrderBy(e => e.PositionLevelId);
        }
    }
}




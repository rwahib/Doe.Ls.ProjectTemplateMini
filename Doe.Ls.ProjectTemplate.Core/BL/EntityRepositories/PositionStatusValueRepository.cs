

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
    public partial class PositionStatusValueRepository : BaseRepository<PositionStatusValue> 
    {
        public PositionStatusValueRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<PositionStatusValue> List()
        {                       
            return base.List()
                    .OrderBy(ent=>ent.PosStatusCode);
        }

        public override void Insert(PositionStatusValue entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(PositionStatusValue entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<PositionStatusValue> FilterPositionStatusValues(IQueryable<PositionStatusValue> positionStatusValues, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredPositionStatusValue = positionStatusValues.Where(ent => 
                    (!string.IsNullOrEmpty(ent.PosStatusCode) && ent.PosStatusCode.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.PosStatusTitle) && ent.PosStatusTitle.ToLower().Contains(searchWord))
);

            return filteredPositionStatusValue.OrderBy(e => e.PosStatusCode);
        }
    }
}




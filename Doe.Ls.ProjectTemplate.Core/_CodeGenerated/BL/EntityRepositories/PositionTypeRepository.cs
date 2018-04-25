 


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
    public partial class PositionTypeRepository : BaseRepository<PositionType> 
    {
        public PositionTypeRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<PositionType> List()
        {                       
            return base.List()
                    .Include(ent=>ent.PositionInformations) 
                    .OrderBy(ent=>ent.PositionTypeCode);
        }

        public override void Insert(PositionType entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(PositionType entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<PositionType> FilterPositionTypes(IQueryable<PositionType> positionTypes, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredPositionType = positionTypes.Where(ent => 
                    (!string.IsNullOrEmpty(ent.PositionTypeCode) && ent.PositionTypeCode.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.PositionTypeName) && ent.PositionTypeName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.PositionTypeDescription) && ent.PositionTypeDescription.ToLower().Contains(searchWord))
);

            return filteredPositionType.OrderBy(e => e.PositionTypeCode);
        }
    }
}




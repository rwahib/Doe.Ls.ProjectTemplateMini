

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
    public partial class OccupationTypeRepository : BaseRepository<OccupationType> 
    {
        public OccupationTypeRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<OccupationType> List()
        {                       
            return base.List()
                    .Include(ent=>ent.PositionInformations) 
                    .OrderBy(ent=>ent.OccupationTypeCode);
        }

        public override void Insert(OccupationType entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(OccupationType entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<OccupationType> FilterOccupationTypes(IQueryable<OccupationType> occupationTypes, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredOccupationType = occupationTypes.Where(ent => 
                    (!string.IsNullOrEmpty(ent.OccupationTypeCode) && ent.OccupationTypeCode.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.OccupationTypeName) && ent.OccupationTypeName.ToLower().Contains(searchWord))
);

            return filteredOccupationType.OrderBy(e => e.OccupationTypeCode);
        }
    }
}




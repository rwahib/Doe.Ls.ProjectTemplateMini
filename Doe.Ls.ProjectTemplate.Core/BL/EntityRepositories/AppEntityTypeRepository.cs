

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
    public partial class AppEntityTypeRepository : BaseRepository<AppEntityType> 
    {
        public AppEntityTypeRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<AppEntityType> List()
        {                       
            return base.List()
                    .OrderBy(ent=>ent.EntityTypeId);
        }

        public override void Insert(AppEntityType entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(AppEntityType entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<AppEntityType> FilterAppEntityTypes(IQueryable<AppEntityType> appEntityTypes, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredAppEntityType = appEntityTypes.Where(ent => 
                    ent.EntityTypeId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.EntityApiName) && ent.EntityApiName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.EntityTitle) && ent.EntityTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.EntityDescription) && ent.EntityDescription.ToLower().Contains(searchWord))
);

            return filteredAppEntityType.OrderBy(e => e.EntityTypeId);
        }
    }
}




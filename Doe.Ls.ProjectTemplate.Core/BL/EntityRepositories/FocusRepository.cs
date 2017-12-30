

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
    public partial class FocusRepository : BaseRepository<Focus> 
    {
        public FocusRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<Focus> List()
        {                       
            return base.List()
                    .Include(ent=>ent.LookupFocusGradeCriterias).Where(ent => ent.FocusId != -1)
                    .OrderBy(ent=>ent.FocusId); ;
        }

        public override void Insert(Focus entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(Focus entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<Focus> FilterFocuss(IQueryable<Focus> focuss, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredFocus = focuss.Where(ent => 
                    ent.FocusId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.FocusName) && ent.FocusName.ToLower().Contains(searchWord))
                    || ent.OrderList.ToString().Contains(searchWord)                    
);

            return filteredFocus.OrderBy(e => e.FocusId);
        }
    }
}




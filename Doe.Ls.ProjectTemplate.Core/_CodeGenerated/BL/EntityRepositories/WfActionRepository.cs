

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
    public partial class WfActionRepository : BaseRepository<WfAction> 
    {
        public WfActionRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<WfAction> List()
        {                       
            return base.List()
                    .OrderBy(ent=>ent.WfActionId);
        }

        public override void Insert(WfAction entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(WfAction entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<WfAction> FilterWfActions(IQueryable<WfAction> wfActions, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredWfAction = wfActions.Where(ent => 
                    ent.WfActionId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.WfActionName) && ent.WfActionName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.WfActionStatus) && ent.WfActionStatus.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.WfActionDescription) && ent.WfActionDescription.ToLower().Contains(searchWord))
);

            return filteredWfAction.OrderBy(e => e.WfActionId);
        }
    }
}




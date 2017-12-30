

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
    public partial class StatusValueRepository : BaseRepository<StatusValue> 
    {
        public StatusValueRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<StatusValue> List()
        {                       
            return base.List()                    
                    .OrderBy(ent=>ent.StatusId);
        }
        public List<StatusValue> ApprovedDeletedList()
        {

            var statusList = new List<int>()
            {
                Enums.StatusValue.Approved.ToInteger(),
                Enums.StatusValue.Deleted.ToInteger()
            };

            return base.List().Where(s => statusList.Contains(s.StatusId)).OrderByDescending(s=>s.StatusId).ToList();

        }
        public List<StatusValue> ActiveNotActiveList()
            {

            var statusList = new List<int>()
            {
                Enums.StatusValue.Active.ToInteger(),
                Enums.StatusValue.Inactive.ToInteger()
            };

            return base.List().Where(s => statusList.Contains(s.StatusId)).OrderByDescending(s => s.StatusId).ToList();

            }
        public override void Insert(StatusValue entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(StatusValue entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        public IQueryable<StatusValue> FilterStatusValues(IQueryable<StatusValue> statusValues, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredStatusValue = statusValues.Where(ent => 
                    (!string.IsNullOrEmpty(ent.StatusName) && ent.StatusName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.StatusDescription) && ent.StatusDescription.ToLower().Contains(searchWord))
);

            return filteredStatusValue.OrderBy(e => e.StatusId);
        }


    }
}




 


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
    public partial class AppObjectInfoRepository : BaseRepository<AppObjectInfo> 
    {
        public AppObjectInfoRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<AppObjectInfo> List()
        {                       
            return base.List()
                    .OrderBy(ent=>ent.ObjectName);
        }

        public override void Insert(AppObjectInfo entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(AppObjectInfo entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<AppObjectInfo> FilterAppObjectInfos(IQueryable<AppObjectInfo> appObjectInfos, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredAppObjectInfo = appObjectInfos.Where(ent => 
                    (!string.IsNullOrEmpty(ent.ObjectName) && ent.ObjectName.ToLower().Contains(searchWord))
                    || ent.CounterValue.ToString().Contains(searchWord)
                    || ent.AggregatedValueA.ToString().Contains(searchWord)
                    || ent.AggregatedValueB.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Metadata) && ent.Metadata.ToLower().Contains(searchWord))
);

            return filteredAppObjectInfo.OrderBy(e => e.ObjectName);
        }
    }
}




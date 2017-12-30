

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
    public partial class GeneralLogRepository : BaseRepository<GeneralLog> 
    {
        public GeneralLogRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<GeneralLog> List()
        {                       
            return base.List()
                    .Include(ent=>ent.SysRole) 
                    .OrderBy(ent=>ent.LogId);
        }

        public override void Insert(GeneralLog entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(GeneralLog entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<GeneralLog> FilterGeneralLogs(IQueryable<GeneralLog> generalLogs, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredGeneralLog = generalLogs.Where(ent => 
                    ent.LogId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Action) && ent.Action.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Context) && ent.Context.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Username) && ent.Username.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.SysRole.RoleApiName) && ent.SysRole.RoleApiName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Note) && ent.Note.ToLower().Contains(searchWord))
);

            return filteredGeneralLog.OrderBy(e => e.LogId);
        }
    }
}




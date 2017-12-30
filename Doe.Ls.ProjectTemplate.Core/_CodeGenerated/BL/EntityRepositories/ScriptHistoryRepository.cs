

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
    public partial class ScriptHistoryRepository : BaseRepository<ScriptHistory> 
    {
        public ScriptHistoryRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<ScriptHistory> List()
        {                       
            return base.List()
                    .OrderBy(ent=>ent.ScriptNumber);
        }

        public override void Insert(ScriptHistory entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(ScriptHistory entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<ScriptHistory> FilterScriptHistorys(IQueryable<ScriptHistory> scriptHistorys, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredScriptHistory = scriptHistorys.Where(ent => 
                    (!string.IsNullOrEmpty(ent.ScriptNumber) && ent.ScriptNumber.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.ScriptName) && ent.ScriptName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.RunBy) && ent.RunBy.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Comments) && ent.Comments.ToLower().Contains(searchWord))
);

            return filteredScriptHistory.OrderBy(e => e.ScriptNumber);
        }
    }
}




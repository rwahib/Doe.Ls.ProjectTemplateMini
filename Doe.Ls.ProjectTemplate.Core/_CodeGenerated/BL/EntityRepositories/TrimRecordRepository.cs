

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
    public partial class TrimRecordRepository : BaseRepository<TrimRecord> 
    {
        public TrimRecordRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<TrimRecord> List()
        {                       
            return base.List()
                    .Include(ent=>ent.RolePositionDescription) 
                    .OrderBy(ent=>ent.RolePositionDescId);
        }

        public override void Insert(TrimRecord entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(TrimRecord entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<TrimRecord> FilterTrimRecords(IQueryable<TrimRecord> trimRecords, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredTrimRecord = trimRecords.Where(ent => 
                    (!string.IsNullOrEmpty(ent.RolePositionDescription.Title) && ent.RolePositionDescription.Title.ToLower().Contains(searchWord))
                    || ent.Uri.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Token) && ent.Token.ToLower().Contains(searchWord))
                    || ent.LastRevisionNumber.ToString().Contains(searchWord)
);

            return filteredTrimRecord.OrderBy(e => e.RolePositionDescId);
        }
    }
}




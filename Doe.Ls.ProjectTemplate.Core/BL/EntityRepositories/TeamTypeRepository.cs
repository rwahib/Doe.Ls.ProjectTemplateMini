

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
    public partial class TeamTypeRepository : BaseRepository<TeamType> 
    {
        public TeamTypeRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<TeamType> List()
        {                       
            return base.List()
                    .Include(ent=>ent.Units) 
                    .OrderBy(ent=>ent.TeamTypeId);
        }

        public override void Insert(TeamType entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(TeamType entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<TeamType> FilterTeamTypes(IQueryable<TeamType> teamTypes, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredTeamType = teamTypes.Where(ent => 
                    ent.TeamTypeId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.TeamTypeName) && ent.TeamTypeName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.TeamTypeDescription) && ent.TeamTypeDescription.ToLower().Contains(searchWord))
);

            return filteredTeamType.OrderBy(e => e.TeamTypeId);
        }
    }
}




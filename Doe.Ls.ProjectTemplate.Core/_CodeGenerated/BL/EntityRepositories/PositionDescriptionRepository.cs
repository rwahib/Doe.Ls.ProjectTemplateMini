

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
    public partial class PositionDescriptionRepository : BaseRepository<PositionDescription> 
    {
        public PositionDescriptionRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<PositionDescription> List()
        {                       
            return base.List()
                    .Include(ent=>ent.RolePositionDescription) 
                    .Include(ent=>ent.PositionFocusCriterias) 
                    .OrderBy(ent=>ent.PositionDescriptionId);
        }

        public override void Insert(PositionDescription entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(PositionDescription entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<PositionDescription> FilterPositionDescriptions(IQueryable<PositionDescription> positionDescriptions, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredPositionDescription = positionDescriptions.Where(ent => 
                    (!string.IsNullOrEmpty(ent.RolePositionDescription.Title) && ent.RolePositionDescription.Title.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.BriefRoleStatement) && ent.BriefRoleStatement.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.StatementOfDuties) && ent.StatementOfDuties.ToLower().Contains(searchWord))
);

            return filteredPositionDescription.OrderBy(e => e.PositionDescriptionId);
        }
    }
}




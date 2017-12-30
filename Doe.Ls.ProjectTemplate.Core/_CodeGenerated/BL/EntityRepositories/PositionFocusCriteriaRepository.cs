

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
    public partial class PositionFocusCriteriaRepository : BaseRepository<PositionFocusCriteria> 
    {
        public PositionFocusCriteriaRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<PositionFocusCriteria> List()
        {                       
            return base.List()
                    .Include(ent=>ent.LookupFocusGradeCriteria) 
                    .Include(ent=>ent.PositionDescription) 
                    .OrderBy(ent=>ent.PositionDescriptionId);
        }

        public override void Insert(PositionFocusCriteria entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(PositionFocusCriteria entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<PositionFocusCriteria> FilterPositionFocusCriterias(IQueryable<PositionFocusCriteria> positionFocusCriterias, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredPositionFocusCriteria = positionFocusCriterias.Where(ent => 
                    (!string.IsNullOrEmpty(ent.PositionDescription.BriefRoleStatement) && ent.PositionDescription.BriefRoleStatement.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.LookupFocusGradeCriteria.GradeCode) && ent.LookupFocusGradeCriteria.GradeCode.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.LookupCustomContent) && ent.LookupCustomContent.ToLower().Contains(searchWord))
);

            return filteredPositionFocusCriteria.OrderBy(e => e.PositionDescriptionId);
        }
    }
}




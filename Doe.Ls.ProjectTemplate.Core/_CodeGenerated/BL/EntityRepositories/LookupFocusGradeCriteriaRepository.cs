

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
    public partial class LookupFocusGradeCriteriaRepository : BaseRepository<LookupFocusGradeCriteria> 
    {
        public LookupFocusGradeCriteriaRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<LookupFocusGradeCriteria> List()
        {                       
            return base.List()
                    .Include(ent=>ent.Focus) 
                    .Include(ent=>ent.Grade) 
                    .Include(ent=>ent.SelectionCriteria) 
                    .Include(ent=>ent.PositionFocusCriterias) 
                    .OrderBy(ent=>ent.LookupId);
        }

        public override void Insert(LookupFocusGradeCriteria entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(LookupFocusGradeCriteria entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<LookupFocusGradeCriteria> FilterLookupFocusGradeCriterias(IQueryable<LookupFocusGradeCriteria> lookupFocusGradeCriterias, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredLookupFocusGradeCriteria = lookupFocusGradeCriterias.Where(ent => 
                    ent.LookupId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Focus.FocusName) && ent.Focus.FocusName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Grade.GradeTitle) && ent.Grade.GradeTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.SelectionCriteria.Criteria) && ent.SelectionCriteria.Criteria.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.LastUpdatedBy) && ent.LastUpdatedBy.ToLower().Contains(searchWord))
);

            return filteredLookupFocusGradeCriteria.OrderBy(e => e.LookupId);
        }
    }
}






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
        public ServiceRepository ServiceRepository
        {
            get
            {
                return new ServiceRepository(this.RepositoryFactory);
            }
        }
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
            entity.LastUpdatedDate = DateTime.Now;
            
            if (string.IsNullOrWhiteSpace(entity.LastUpdatedBy))
            {
                entity.LastUpdatedBy = SessionService.GetCurrentUser().UserName;
            }
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
                    ||  ent.IsMandatory.ToString().ToLower().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.SelectionCriteria.Criteria) && ent.SelectionCriteria.Criteria.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.LastUpdatedBy) && ent.LastUpdatedBy.ToLower().Contains(searchWord))

);

            return filteredLookupFocusGradeCriteria.OrderBy(e => e.LookupId);
        }
        public object LoadFocusListNotInLookup(string gradeCode)
        {
           
            var existingFocus = this.List().Where(l => l.GradeCode == gradeCode).Select(l => l.FocusId);
            var focusList = ServiceRepository.FocusRepository().List().Where(l=>!existingFocus.Contains(l.FocusId)).ToList();
            return focusList.Select(y => new { Value = y.FocusId, Text = y.FocusName});
             
        }

        public object LoadCriteriaListByFocusAndGrade(string gradeCode, int focusId)
        {
            var existingCriterias = this.List().Where(l => l.GradeCode == gradeCode && l.FocusId ==focusId).Select(l => l.SelectionCriteriaId);
            var criteriaList = ServiceRepository.SelectionCriteriaRepository().List().Where(l => !existingCriterias.Contains(l.SelectionCriteriaId)).ToList();
            return criteriaList.Select(y => new { Value = y.SelectionCriteriaId, Text = y.Criteria });
        }
    }
}






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
    public partial class SelectionCriteriaRepository : BaseRepository<SelectionCriteria> 
    {
        public SelectionCriteriaRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<SelectionCriteria> List()
        {                       
            return base.List()
                    .Include(ent=>ent.LookupFocusGradeCriterias) 
                    .OrderBy(ent=>ent.SelectionCriteriaId);
        }

        public override void Insert(SelectionCriteria entity) 
        {
            entity.LastModifiedDate = DateTime.Now;
            var id =  this.GetDbNewId("SelectionCriteria");
            entity.SelectionCriteriaId = id;
            
            if (string.IsNullOrWhiteSpace(entity.LastModifiedBy))
            {
                entity.LastModifiedBy = SessionService.GetCurrentUser().UserName;
            }

            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(SelectionCriteria entity, bool refresh = true) 
        {
            entity.LastModifiedDate = DateTime.Now;
           
            if (string.IsNullOrWhiteSpace(entity.LastModifiedBy))
            {
                entity.LastModifiedBy = SessionService.GetCurrentUser().UserName;
            }
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<SelectionCriteria> FilterSelectionCriterias(IQueryable<SelectionCriteria> selectionCriterias, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredSelectionCriteria = selectionCriterias.Where(ent => 
                    ent.SelectionCriteriaId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Criteria) && ent.Criteria.ToLower().Contains(searchWord))
);

            return filteredSelectionCriteria.OrderBy(e => e.SelectionCriteriaId);
        }

    }
}




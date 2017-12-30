

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
    public partial class RelationshipScopeRepository : BaseRepository<RelationshipScope> 
    {
        public RelationshipScopeRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<RelationshipScope> List()
        {                       
            return base.List()
                    .Include(ent=>ent.KeyRelationships) 
                    .OrderBy(ent=>ent.ScopeId);
        }

        public override void Insert(RelationshipScope entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(RelationshipScope entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<RelationshipScope> FilterRelationshipScopes(IQueryable<RelationshipScope> relationshipScopes, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredRelationshipScope = relationshipScopes.Where(ent => 
                    ent.ScopeId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.ScopeTitle) && ent.ScopeTitle.ToLower().Contains(searchWord))
);

            return filteredRelationshipScope.OrderBy(e => e.ScopeId);
        }
    }
}






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
    public partial class KeyRelationshipRepository : BaseRepository<KeyRelationship> 
    {
        public KeyRelationshipRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public IQueryable<KeyRelationship> BaseList()
        {
            return base.List()
                .Include(ent => ent.RelationshipScope);

        }

        public KeyRelationship GetPrimitiveById(int keyRelationshipId)
        {
            return base.List().FirstOrDefault(k => k.KeyRelationshipId == keyRelationshipId);
        }

        public override IQueryable<KeyRelationship> List()
        {                       
            return BaseList()
                    .Include(ent=>ent.RelationshipScope) 
                    .Include(ent=>ent.RoleDescription) 
                    .OrderBy(ent=>ent.KeyRelationshipId);
        }

        public IEnumerable<KeyRelationship> LoadKeyRelationshipByRoleDescId(int roleDescId)
        {
            return List().Where(k => k.RoleDescriptionId == roleDescId).ToList();
        }

        public KeyRelationship GetKeyRelationshipWithRoleDescById(int keyRelationshipId)
        {
            return List().FirstOrDefault(k => k.KeyRelationshipId == keyRelationshipId);
        }

        public override void Insert(KeyRelationship entity) 
        {
            entity.DateCreated = DateTime.Now;
            entity.LastUpdated = DateTime.Now;
            //entity.OrderNumber = 1;
            if (string.IsNullOrWhiteSpace(entity.ModifiedUserName))
            {
                entity.ModifiedUserName = SessionService.GetCurrentUser().UserName;
            }
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(KeyRelationship entity, bool refresh = true) 
        {
           
            entity.LastUpdated = DateTime.Now;
            if (string.IsNullOrWhiteSpace(entity.ModifiedUserName))
            {
                entity.ModifiedUserName = SessionService.GetCurrentUser().UserName;
            }


            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }


        public IQueryable<KeyRelationship> FilterKeyRelationships(IQueryable<KeyRelationship> keyRelationships, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredKeyRelationship = keyRelationships.Where(ent => 
                    ent.KeyRelationshipId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.RoleDescription.OldPDFileName) && ent.RoleDescription.OldPDFileName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.RelationshipScope.ScopeTitle) && ent.RelationshipScope.ScopeTitle.ToLower().Contains(searchWord))
                    || ent.OrderNumber.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Who) && ent.Who.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Why) && ent.Why.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.ModifiedUserName) && ent.ModifiedUserName.ToLower().Contains(searchWord))
);

            return filteredKeyRelationship.OrderBy(e => e.KeyRelationshipId);
        }
    }
}




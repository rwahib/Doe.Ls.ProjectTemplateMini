 


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
    public partial class RolePositionDescriptionRepository : BaseRepository<RolePositionDescription> 
    {
        public RolePositionDescriptionRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<RolePositionDescription> List()
        {                       
            return base.List()
                    .Include(ent=>ent.Grade) 
                    .Include(ent=>ent.PositionDescription) 
                    .Include(ent=>ent.RoleDescription) 
                    .Include(ent=>ent.StatusValue) 
                    .Include(ent=>ent.RolePositionDescriptionHistories) 
                    .Include(ent=>ent.TrimRecord) 
                    .Include(ent=>ent.Positions) 
                    .OrderBy(ent=>ent.RolePositionDescId);
        }

        public override void Insert(RolePositionDescription entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(RolePositionDescription entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<RolePositionDescription> FilterRolePositionDescriptions(IQueryable<RolePositionDescription> rolePositionDescriptions, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredRolePositionDescription = rolePositionDescriptions.Where(ent => 
                    ent.RolePositionDescId.ToString().Contains(searchWord)
                    || ent.Version.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Title) && ent.Title.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.DocNumber) && ent.DocNumber.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Grade.GradeTitle) && ent.Grade.GradeTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredRolePositionDescription.OrderBy(e => e.RolePositionDescId);
        }
    }
}




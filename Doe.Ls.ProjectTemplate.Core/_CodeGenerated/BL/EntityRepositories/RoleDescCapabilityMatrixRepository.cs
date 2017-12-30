

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
    public partial class RoleDescCapabilityMatrixRepository : BaseRepository<RoleDescCapabilityMatrix> 
    {
        public RoleDescCapabilityMatrixRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<RoleDescCapabilityMatrix> List()
        {                       
            return base.List()
                    .Include(ent=>ent.Grade) 
                    .OrderBy(ent=>ent.GradeCode);
        }

        public override void Insert(RoleDescCapabilityMatrix entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(RoleDescCapabilityMatrix entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<RoleDescCapabilityMatrix> FilterRoleDescCapabilityMatrixs(IQueryable<RoleDescCapabilityMatrix> roleDescCapabilityMatrixs, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredRoleDescCapabilityMatrix = roleDescCapabilityMatrixs.Where(ent => 
                    (!string.IsNullOrEmpty(ent.Grade.GradeTitle) && ent.Grade.GradeTitle.ToLower().Contains(searchWord))
                    || ent.Foundational_Min.ToString().Contains(searchWord)
                    || ent.Foundational_Max.ToString().Contains(searchWord)
                    || ent.Intermediate_Min.ToString().Contains(searchWord)
                    || ent.Intermediate_Max.ToString().Contains(searchWord)
                    || ent.Adept_Min.ToString().Contains(searchWord)
                    || ent.Adept_Max.ToString().Contains(searchWord)
                    || ent.Advanced_Min.ToString().Contains(searchWord)
                    || ent.Advanced_Max.ToString().Contains(searchWord)
                    || ent.HighlyAdvanced_Min.ToString().Contains(searchWord)
                    || ent.HighlyAdvanced_Max.ToString().Contains(searchWord)
                    || ent.FocusCapabilities_Min.ToString().Contains(searchWord)
                    || ent.FocusCapabilities_Max.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Notes) && ent.Notes.ToLower().Contains(searchWord))
);

            return filteredRoleDescCapabilityMatrix.OrderBy(e => e.GradeCode);
        }
    }
}






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
                    .OrderBy(ent=>ent.GradeCode);
        }

        public RoleDescCapabilityMatrix LoadMatrixByGrade(string gradeCode)
        {
             return base.List().FirstOrDefault(m => m.GradeCode == gradeCode);
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
                    (!string.IsNullOrEmpty(ent.GradeCode) && ent.GradeCode.ToLower().Contains(searchWord))
                 || (!string.IsNullOrEmpty(ent.Notes) && ent.Notes.ToLower().Contains(searchWord))
);

            return filteredRoleDescCapabilityMatrix.OrderBy(e => e.GradeCode);
        }
    }
}




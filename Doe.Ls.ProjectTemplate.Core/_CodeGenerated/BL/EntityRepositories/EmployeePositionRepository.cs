

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
    public partial class EmployeePositionRepository : BaseRepository<EmployeePosition> 
    {
        public EmployeePositionRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<EmployeePosition> List()
        {                       
            return base.List()
                    .Include(ent=>ent.Employee) 
                    .Include(ent=>ent.Position) 
                    .OrderBy(ent=>ent.EmployeeId);
        }

        public override void Insert(EmployeePosition entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(EmployeePosition entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<EmployeePosition> FilterEmployeePositions(IQueryable<EmployeePosition> employeePositions, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredEmployeePosition = employeePositions.Where(ent => 
                    (!string.IsNullOrEmpty(ent.Employee.FirstName) && ent.Employee.FirstName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Position.PositionTitle) && ent.Position.PositionTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Reason) && ent.Reason.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredEmployeePosition.OrderBy(e => e.EmployeeId);
        }
    }
}




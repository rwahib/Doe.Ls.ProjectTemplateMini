 


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
    public partial class EmployeeRepository : BaseRepository<Employee> 
    {
        public EmployeeRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<Employee> List()
        {                       
            return base.List()
                    .Include(ent=>ent.EmployeePositions) 
                    .OrderBy(ent=>ent.EmployeeId);
        }

        public override void Insert(Employee entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(Employee entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<Employee> FilterEmployees(IQueryable<Employee> employees, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredEmployee = employees.Where(ent => 
                    ent.EmployeeId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.FirstName) && ent.FirstName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.LastName) && ent.LastName.ToLower().Contains(searchWord))
                    || ent.CostCentreNumber.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredEmployee.OrderBy(e => e.EmployeeId);
        }
    }
}




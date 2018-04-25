 


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
    public partial class EmployeeTypeRepository : BaseRepository<EmployeeType> 
    {
        public EmployeeTypeRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<EmployeeType> List()
        {                       
            return base.List()
                    .Include(ent=>ent.PositionInformations) 
                    .OrderBy(ent=>ent.EmployeeTypeCode);
        }

        public override void Insert(EmployeeType entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(EmployeeType entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<EmployeeType> FilterEmployeeTypes(IQueryable<EmployeeType> employeeTypes, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredEmployeeType = employeeTypes.Where(ent => 
                    (!string.IsNullOrEmpty(ent.EmployeeTypeCode) && ent.EmployeeTypeCode.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.EmployeeTypeName) && ent.EmployeeTypeName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.EmployeeTypeDescription) && ent.EmployeeTypeDescription.ToLower().Contains(searchWord))
);

            return filteredEmployeeType.OrderBy(e => e.EmployeeTypeCode);
        }
    }
}




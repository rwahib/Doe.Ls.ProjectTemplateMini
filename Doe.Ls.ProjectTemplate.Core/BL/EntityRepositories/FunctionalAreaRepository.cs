

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
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories 
{
    public partial class FunctionalAreaRepository : BaseRepository<FunctionalArea> 
    {
        public FunctionalAreaRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<FunctionalArea> List()
        {
            
            return this.BasicList().Include(ent=>ent.Directorate)
                    .Include(ent=>ent.Directorate.Executive) 
                    .Include(ent=>ent.StatusValue) 
                    .Include(ent=>ent.Units) 
                    .OrderBy(ent=>ent.FuncationalAreaId);
        }

        public IQueryable<FunctionalArea> BasicList()
            {
            var na = Enums.Cnt.Na;
            return base.List().Where(fa => fa.FuncationalAreaId != na);

            }
        public FunctionalArea GetFunctionalAreaById(int funcationalAreaId)
            {
            return List().SingleOrDefault(e => e.FuncationalAreaId == funcationalAreaId);

            }
        public override void Insert(FunctionalArea entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(FunctionalArea entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<FunctionalArea> FilterFunctionalAreas(IQueryable<FunctionalArea> functionalAreas, BasicStructureArgument searchArg)
        {
            var searchWord = searchArg.sSearch?.ToLower();
            if (searchArg.DirectorateId > 0)
                functionalAreas = functionalAreas.Where(d => d.DirectorateId == searchArg.DirectorateId);
            if(!string.IsNullOrWhiteSpace(searchArg.DivisionCode)) functionalAreas = functionalAreas.Where(d => d.Directorate.Executive.ExecutiveCod == searchArg.DivisionCode);           
            if (!string.IsNullOrWhiteSpace(searchWord))
            {
                functionalAreas = functionalAreas.Where(ent =>
                    ent.FuncationalAreaId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.AreanName) && ent.AreanName.ToLower().Contains(searchWord))
                    ||
                    (!string.IsNullOrEmpty(ent.AreaDescription) && ent.AreaDescription.ToLower().Contains(searchWord))
                    
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.LastModifiedBy) && ent.LastModifiedBy.ToLower().Contains(searchWord))
                    );
            }
            return functionalAreas.OrderBy(e => e.FuncationalAreaId);
        }

      
    }
}




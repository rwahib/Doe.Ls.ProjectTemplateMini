 


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
    public partial class GradeRepository : BaseRepository<Grade> 
    {
        public GradeRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<Grade> List()
        {                       
            return base.List()
                    .Include(ent=>ent.StatusValue) 
                    .Include(ent=>ent.LookupFocusGradeCriterias) 
                    .Include(ent=>ent.RoleDescCapabilityMatrix) 
                    .Include(ent=>ent.RolePositionDescriptions) 
                    .OrderBy(ent=>ent.GradeCode);
        }

        public override void Insert(Grade entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(Grade entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<Grade> FilterGrades(IQueryable<Grade> grades, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredGrade = grades.Where(ent => 
                    (!string.IsNullOrEmpty(ent.GradeCode) && ent.GradeCode.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.GradeTitle) && ent.GradeTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Award) && ent.Award.ToLower().Contains(searchWord))
                    || ent.AwardMaxRates.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.GradeType) && ent.GradeType.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Message) && ent.Message.ToLower().Contains(searchWord))
);

            return filteredGrade.OrderBy(e => e.GradeCode);
        }
    }
}






using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.CompilerServices;
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
    public partial class GradeRepository : BaseRepository<Grade> 
    {
        public GradeRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }
        public IQueryable<Grade> BasicList()
            {
         return base.List().Where(d => d.GradeCode != Enums.Grade.NA);

            }
        public override IQueryable<Grade> List()
        {
            return BasicList()
                .Include(ent => ent.StatusValue)
                .OrderBy(ent => ent.GradeCode);
        }

        public IQueryable<Grade> ListForPositiondesc()
        {
            return List()
                    .Include(ent => ent.StatusValue)                    
                    .Where(l => l.GradeType == Enums.GradeType.NSBTS.ToString())
                    .OrderBy(ent => ent.GradeCode);
        }
        public IQueryable<Grade> GradeOnType(bool teachingBased)
        {
            return base.List()
                    .Include(ent => ent.StatusValue)                    
                    .Where(l => l.GradeCode != "-1" && l.TeachingBased==teachingBased)
                    .OrderBy(ent => ent.GradeCode);
        }

        public Grade GetGradeByCode(string gradeCode)
        {
            return List().SingleOrDefault(g => g.GradeCode == gradeCode);
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
        public IQueryable<Grade> FilterGrades(IQueryable<Grade> grades, BasicArgument arg)
        {

            var searchWord = arg.sSearch;
            var filteredGrade = grades;
            
            if(arg.StatusId > 0)
                {
                filteredGrade = filteredGrade.Where(g => g.StatusId == arg.StatusId);
                }
            if(!string.IsNullOrWhiteSpace(arg.GradeType))
                {
                filteredGrade = filteredGrade.Where(g => g.GradeType== arg.GradeType);
                }
            if(arg.StatusCode.HasAnyValue())
                {                
                filteredGrade = filteredGrade.Where(g => arg.StatusCode.Contains(g.StatusId.ToString()));
                }
            if (!string.IsNullOrWhiteSpace(searchWord))
            {
                filteredGrade = filteredGrade.Where(ent =>
                    (!string.IsNullOrEmpty(ent.GradeCode) && ent.GradeCode.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.GradeTitle) && ent.GradeTitle.ToLower().Contains(searchWord))                    
                    );
            }
            return filteredGrade.OrderBy(e => e.GradeCode);
        }


        public bool IsTeachingBased(string gradeCode)
        {
            var grade = this.GetGradeByCode(gradeCode);
            if (grade.TeachingBased != null && grade.TeachingBased==true)
                return true;

            return false;
        }


        public IEnumerable<Enums.GradeType> GetGradeTypeList()
        {
            return Enum.GetValues(typeof(Enums.GradeType)).OfType<Enums.GradeType>();
            
        }
    }
}




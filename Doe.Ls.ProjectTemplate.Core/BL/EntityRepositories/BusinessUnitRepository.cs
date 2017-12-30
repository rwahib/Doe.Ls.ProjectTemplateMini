

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
    public partial class BusinessUnitRepository : BaseRepository<BusinessUnit>
        {
        public BusinessUnitRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
            {
            }

        public override IQueryable<BusinessUnit> List()
            {
            return BasicList()
                     .Include(ent => ent.Directorate)
                    .Include(ent => ent.Directorate.Executive)
                    .Include(ent => ent.HierarchyLevel)
                    .Include(ent => ent.StatusValue)
                    .Include(ent => ent.Units)
                    .OrderBy(ent => ent.BUnitId);
            }
        public IQueryable<BusinessUnit> BasicList()
            {
            var na = Enums.Cnt.Na;
            return base.List().Where(d => d.BUnitId != na);

            }
        public BusinessUnit GetBUnitById(int id)
            {
            return List().SingleOrDefault(e => e.BUnitId== id);

            }
        public override void Insert(BusinessUnit entity)
            {

            if(ValidateEntity(entity).Count > 0)
                {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
                }

            base.Insert(entity);
            }

        public override void Update(BusinessUnit entity, bool refresh = true)
            {

            if(ValidateEntity(entity).Count > 0)
                {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
                }

            base.Update(entity, refresh);
            }
        
        public IQueryable<BusinessUnit> FilterBusinessUnits(IQueryable<BusinessUnit> businessUnits, BasicStructureArgument arg)
        {
            var searchWord = arg.sSearch;
            var filteredBusinessUnit = businessUnits;
            if (arg.DirectorateId > 0)
            {
                filteredBusinessUnit = filteredBusinessUnit.Where(bu => bu.DirectorateId == arg.DirectorateId);
            }
            if(!string.IsNullOrWhiteSpace(arg.DivisionCode))
                {
                filteredBusinessUnit = filteredBusinessUnit.Where(bu => bu.Directorate.ExecutiveCod == arg.DivisionCode);
                }

            if (!string.IsNullOrWhiteSpace(searchWord))
            {
                filteredBusinessUnit = filteredBusinessUnit.Where(ent =>
                    ent.BUnitId.ToString().Contains(searchWord)
                    ||
                    (!string.IsNullOrEmpty(ent.Directorate.Executive.ExecutiveTitle) &&
                     ent.Directorate.Executive.ExecutiveTitle.ToLower().Contains(searchWord))
                    ||
                    (!string.IsNullOrEmpty(ent.Directorate.DirectorateName) &&
                     ent.Directorate.DirectorateName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.BUnitName) && ent.BUnitName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.LastModifiedBy) && ent.LastModifiedBy.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
                    );
            }
            return filteredBusinessUnit.OrderBy(e => e.BUnitId);
            }
        }
    }




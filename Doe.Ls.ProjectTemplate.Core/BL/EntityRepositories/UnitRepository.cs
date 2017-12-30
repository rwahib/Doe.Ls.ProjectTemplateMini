using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories 
{
    public partial class UnitRepository : BaseRepository<Unit> 
    {
        public UnitRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public IQueryable<Unit> BasicList()
            {
            var na = Enums.Cnt.Na;
            return base.List().Where(u => u.UnitId != na);
            }

        public override IQueryable<Unit> List()
        {
          return BasicList()
                    .Include(ent=>ent.BusinessUnit)
                    .Include(ent=>ent.TeamType)
                    .Include(ent=>ent.HierarchyLevel)
                    .Include(ent=>ent.FunctionalArea)
                    .Include(ent=>ent.BusinessUnit.Directorate)
                    .Include(ent=>ent.BusinessUnit.Directorate.Executive)                      
                    .Include(ent=>ent.StatusValue) 
                    .OrderBy(ent=>ent.UnitId);
        }
        public IQueryable<Unit> ListWithPositions()
            {
            
            return this.List()
                    .Include(ent => ent.Positions)
                    .OrderBy(ent => ent.UnitId);
            }
        public Unit GetUnitById(int unitId)
            {
            return List().SingleOrDefault(e => e.UnitId == unitId);

            }
        public override void Insert(Unit entity) 
        {
            if (entity.ReportToUnit <= 0)
            {
                entity.ReportToUnit = Enums.Cnt.Na;
            }

            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(Unit entity, bool refresh = true)
        {
            if (entity.ReportToUnit == 0) entity.ReportToUnit = Enums.Cnt.Na;

            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }
        public IQueryable<Unit> FilterUnits(IQueryable<Unit> units, BasicStructureArgument arg)
        {
            var searchWord = arg.sSearch;
            var filtereTeams = units;
            if(arg.DirectorateId > 0)
                {
                filtereTeams = filtereTeams.Where(t => t.BusinessUnit.DirectorateId == arg.DirectorateId);
                }
            if(arg.BusinessUnitId > 0)
                {
                filtereTeams = filtereTeams.Where(t => t.BUnitId == arg.BusinessUnitId);
                }
            if(arg.FuncationalAreaId > 0)
                {
                filtereTeams = filtereTeams.Where(t => t.FunctionalAreaId == arg.FuncationalAreaId);
                }
            if(!string.IsNullOrWhiteSpace(arg.DivisionCode))
                {
                filtereTeams = filtereTeams.Where(t => t.BusinessUnit.Directorate.ExecutiveCod== arg.DivisionCode);
                }

            if(!string.IsNullOrWhiteSpace(searchWord))
            {
                filtereTeams = filtereTeams.Where(ent =>
                    ent.UnitId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.UnitName) && ent.UnitName.ToLower().Contains(searchWord))
                    ||
                    (!string.IsNullOrEmpty(ent.FunctionalArea.AreanName) &&
                     ent.FunctionalArea.AreanName.ToLower().Contains(searchWord))
                    ||
                    (!string.IsNullOrEmpty(ent.BusinessUnit.BUnitName) &&
                     ent.BusinessUnit.BUnitName.ToLower().Contains(searchWord))
                    ||
                    (!string.IsNullOrEmpty(ent.BusinessUnit.Directorate.DirectorateName) &&
                     ent.BusinessUnit.Directorate.DirectorateName.ToLower().Contains(searchWord))
                    || ent.ReportToUnit.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.LastModifiedBy) && ent.LastModifiedBy.ToLower().Contains(searchWord))
                    );
            }
            return filtereTeams.OrderBy(e => e.UnitId);
        }

        public SelectListItemExtension[] GetUnitsWithPositionCount(int bUnitId)
        {
            var liveStatus = new int[] { Enums.StatusValue.Approved.ToInteger(), Enums.StatusValue.Imported.ToInteger() };

            var units = this.BasicList().Include(u=>u.Positions).Where(l => l.BUnitId == bUnitId).ToArray()
                    .Select(l =>
                    {
                        
                        var count=l.Positions.Count(p => liveStatus.Contains(p.StatusId));
                        return new SelectListItemExtension
                        {
                            Value = l.UnitId.ToString(),
                            Text = $"{l.UnitName} ({count})"
                        };
                    }).ToArray();

                return units;
           
            
        }
        public SelectListItemExtension[] GetUnits(int bUnitId)
        {
           
           
                var units2 = this.List().Where(l => l.BUnitId == bUnitId).ToArray()
                    .Select(l => new SelectListItemExtension
                    {
                        Value = l.UnitId.ToString(),
                        Text = l.UnitName
                    })
                    .ToArray();

            return units2;
        }
    }
}




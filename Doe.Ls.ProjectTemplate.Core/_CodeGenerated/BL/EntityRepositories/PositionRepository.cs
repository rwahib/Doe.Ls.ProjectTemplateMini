

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
    public partial class PositionRepository : BaseRepository<Position> 
    {
        public PositionRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<Position> List()
        {                       
            return base.List()
                    .Include(ent=>ent.CostCentreDetail) 
                    .Include(ent=>ent.EmployeePositions) 
                    .Include(ent=>ent.Location) 
                    .Include(ent=>ent.Positions) 
                    .Include(ent=>ent.ParentPosition) 
                    .Include(ent=>ent.PositionLevel) 
                    .Include(ent=>ent.RolePositionDescription) 
                    .Include(ent=>ent.StatusValue) 
                    .Include(ent=>ent.Unit) 
                    .Include(ent=>ent.PositionHistories) 
                    .Include(ent=>ent.PositionInformation) 
                    .OrderBy(ent=>ent.PositionId);
        }

        public override void Insert(Position entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(Position entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<Position> FilterPositions(IQueryable<Position> positions, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredPosition = positions.Where(ent => 
                    ent.PositionId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Position.PositionTitle) && ent.Position.PositionTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.PositionNumber) && ent.PositionNumber.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.RolePositionDescription.Title) && ent.RolePositionDescription.Title.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Unit.UnitName) && ent.Unit.UnitName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.PositionTitle) && ent.PositionTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Description) && ent.Description.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.PositionLevel.PositionLevelName) && ent.PositionLevel.PositionLevelName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.PositionPath) && ent.PositionPath.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Location.Name) && ent.Location.Name.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.DivisionOverview) && ent.DivisionOverview.ToLower().Contains(searchWord))
);

            return filteredPosition.OrderBy(e => e.PositionId);
        }
    }
}




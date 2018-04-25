 


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
    public partial class PositionInformationRepository : BaseRepository<PositionInformation> 
    {
        public PositionInformationRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<PositionInformation> List()
        {                       
            return base.List()
                    .Include(ent=>ent.EmployeeType) 
                    .Include(ent=>ent.OccupationType) 
                    .Include(ent=>ent.PositionStatusValue) 
                    .Include(ent=>ent.PositionType) 
                    .Include(ent=>ent.PositionNotes) 
                    .Include(ent=>ent.Position) 
                    .OrderBy(ent=>ent.PositionId);
        }

        public override void Insert(PositionInformation entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(PositionInformation entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<PositionInformation> FilterPositionInformations(IQueryable<PositionInformation> positionInformations, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredPositionInformation = positionInformations.Where(ent => 
                    (!string.IsNullOrEmpty(ent.Position.PositionTitle) && ent.Position.PositionTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.OlderPositionNumber3) && ent.OlderPositionNumber3.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.OlderPositionNumber1) && ent.OlderPositionNumber1.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.OlderPositionNumber2) && ent.OlderPositionNumber2.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.SchNumber) && ent.SchNumber.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.PositionType.PositionTypeName) && ent.PositionType.PositionTypeName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.EmployeeType.EmployeeTypeName) && ent.EmployeeType.EmployeeTypeName.ToLower().Contains(searchWord))
                    || ent.PositionNoteId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.TrimLink) && ent.TrimLink.ToLower().Contains(searchWord))
                    || ent.PositionFTE.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.PositionStatusValue.PosStatusTitle) && ent.PositionStatusValue.PosStatusTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.OccupationType.OccupationTypeName) && ent.OccupationType.OccupationTypeName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.OtherOverview) && ent.OtherOverview.ToLower().Contains(searchWord))
);

            return filteredPositionInformation.OrderBy(e => e.PositionId);
        }
    }
}




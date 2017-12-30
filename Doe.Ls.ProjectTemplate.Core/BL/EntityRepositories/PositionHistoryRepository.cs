

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
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories 
{
    public partial class PositionHistoryRepository : BaseRepository<PositionHistory> 
    {
        public PositionHistoryRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<PositionHistory> List()
        {                       
            return base.List()
                    .Include(ent=>ent.Position)
                    .Include(ent=>ent.Position.RolePositionDescription)                    
                    .OrderByDescending(ent=>ent.CreatedDate);
        }

        public IQueryable<PositionHistory> GetHistoryEntriesByPositionId(int positionId)
        {
            return this.List().Where(h => h.PositionId == positionId);

        }

        public PositionHistory GetHistoryEntryById(int positionHistoryId)
            {
            return this.List().SingleOrDefault(h => h.PositionHistoryId == positionHistoryId);
            }

        public override void Insert(PositionHistory entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(PositionHistory entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }


        public void LogHistoryOnClonePositioin(Position position, string sourcePositionNumber,string userName)
        {
            var history = new PositionHistory
            {
                PositionId = position.PositionId,
                Action = Enum.GetName(typeof(Enums.ActionType), Enums.ActionType.Create),
                StatusFrom = "New",

                StatusTo = Enum.GetName(typeof(Enums.StatusValue), position.StatusId),
                AdditionalInfo = "Position was cloned from source position number (" + sourcePositionNumber +").",
                CreatedBy = userName,
                CreatedDate = DateTime.Now
            };

            Insert(history);
        }
        public void LogHistoryOnCreatePositioin(Position position, string userName, string msg = "")
        {
            var history = new PositionHistory
            {
                PositionId = position.PositionId,
                Action = Enum.GetName(typeof(Enums.ActionType), Enums.ActionType.Create),
                StatusFrom = "New",

                StatusTo = Enum.GetName(typeof(Enums.StatusValue), position.StatusId),
                AdditionalInfo = msg,
                CreatedBy = userName,
                CreatedDate = DateTime.Now
            };

            Insert(history);
        }

        public void LogHistoryOtherPositionItems(int positionId,int actionType, int statusFromId,  int statusToId, 
            string additionalInfo, string userName)
        {
            var history = new PositionHistory
            {
                PositionId = positionId,
                Action = Enum.GetName(typeof(Enums.ActionType), actionType),
                StatusFrom = Enum.GetName(typeof(Enums.StatusValue), statusFromId),

                StatusTo = Enum.GetName(typeof(Enums.StatusValue), statusToId),
                AdditionalInfo = additionalInfo,
                CreatedBy = userName,
                CreatedDate = DateTime.Now
            };

            Insert(history);
        }

        public void LogHistoryWhenUpdated(int positionId, int oldStatusId, int newStatusId, StringBuilder sb, string sectionName, string userName)
        {
            //add to history
            var history = new PositionHistory
            {
                PositionId = positionId,
                Action = Enum.GetName(typeof(Enums.ActionType), Enums.ActionType.Update),
                StatusFrom = Enum.GetName(typeof(Enums.StatusValue), oldStatusId),
                StatusTo = Enum.GetName(typeof(Enums.StatusValue), newStatusId),
                AdditionalInfo = sectionName + " has been updated. " + sb.ToString(),
                CreatedBy = userName,
                CreatedDate = DateTime.Now
            };

            Insert(history);
        }

        public PositionHistory GetHistoryWhenUpdated(int positionId, int oldStatusId, int newStatusId, StringBuilder sb, string sectionName, string userName)
        {
            //add to history
            var history = new PositionHistory
            {
                PositionId = positionId,
                Action = Enum.GetName(typeof(Enums.ActionType), Enums.ActionType.Update),
                StatusFrom = Enum.GetName(typeof(Enums.StatusValue), oldStatusId),
                StatusTo = Enum.GetName(typeof(Enums.StatusValue), newStatusId),
                AdditionalInfo = sectionName + " has been updated. " + sb.ToString(),
                CreatedBy = userName,
                CreatedDate = DateTime.Now
            };

            return history;
        }


        public PositionHistory GetHistoryOnCreatePosition(Position position, string userName, string msg)
        {
            var history = new PositionHistory
            {
                PositionId = position.PositionId,
                Action = Enum.GetName(typeof(Enums.ActionType), Enums.ActionType.Create),
                StatusFrom = "New",

                StatusTo = Enum.GetName(typeof(Enums.StatusValue), (int)Enums.StatusValue.Draft),
                AdditionalInfo = msg,
                CreatedBy = userName,
                CreatedDate = DateTime.Now
            };

            return history;
        }

        public IQueryable<PositionHistory> FilterPositionHistorys(IQueryable<PositionHistory> positionHistorys, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredPositionHistory = positionHistorys.Where(ent => 
                    ent.PositionHistoryId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Position.PositionTitle) && ent.Position.PositionTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Action) && ent.Action.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.StatusFrom) && ent.StatusFrom.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.StatusTo) && ent.StatusTo.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.AdditionalInfo) && ent.AdditionalInfo.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredPositionHistory.OrderBy(e => e.PositionHistoryId);
        }



        public StringBuilder GetPositionChanges(Position oldP, Position newP)
        {
            StringBuilder sb = new StringBuilder();

            if (oldP != null)
            {
                sb.Append("Old Position# = '");
                sb.Append(oldP.PositionNumber);
                sb.Append("', ");
                sb.Append("old Position title = '");
                sb.Append(oldP.PositionTitle);
                sb.Append("', ");
                sb.Append("old Division = '");
                sb.Append(oldP.Unit.BusinessUnit.Directorate.Executive.ExecutiveTitle);
                sb.Append("', ");
                sb.Append("old Directorate = '");
                sb.Append(oldP.Unit.BusinessUnit.Directorate.DirectorateName);
                sb.Append("', ");
                sb.Append("old Business unit = '");
                sb.Append(oldP.Unit.BusinessUnit.BUnitName);
                sb.Append("', ");
                sb.Append("old Team = '");
                sb.Append(oldP.Unit.UnitName);
                sb.Append("', ");
                sb.Append("old ReportTo = '");
                sb.Append(oldP.ReportToPositionId);
                sb.Append("', ");
                sb.Append("old Location = '");
                sb.Append(oldP.Location.Name);
                sb.Append("', ");
                sb.Append("old Position level = '");
                sb.Append(oldP.PositionLevel.PositionLevelName);
                sb.Append("'. ");
            }

            
            sb.Append("New Position# = '");
            sb.Append(newP.PositionNumber);
            sb.Append("', ");
            sb.Append("new Position title = '");
            sb.Append(newP.PositionTitle);
            sb.Append("', ");
            sb.Append("new Division = '");
            sb.Append(newP.Unit.BusinessUnit.Directorate.Executive.ExecutiveTitle);
            sb.Append("', ");
            sb.Append("new Directorate = '");
            sb.Append(newP.Unit.BusinessUnit.Directorate.DirectorateName);
            sb.Append("', ");
            sb.Append("new Business unit = '");
            sb.Append(newP.Unit.BusinessUnit.BUnitName);
            sb.Append("', ");
            sb.Append("new Team = '");
            sb.Append(newP.Unit.UnitName);
            sb.Append("', ");
            sb.Append("new ReportTo = '");
            sb.Append(newP.ReportToPositionId);
            sb.Append("', ");
            sb.Append("new Location = '");
            sb.Append(newP.Location.Name);
            sb.Append("', ");
            sb.Append("new Position level = '");
            sb.Append(newP.PositionLevel.PositionLevelName);
            sb.Append("'. ");

            return sb;
        }


        public StringBuilder GetPositionInfoChanges(PositionInformation oldPinfo, PositionInformation newPinfo, 
            string oldNote, string newNote)
        {
            StringBuilder sb = new StringBuilder();

            if (oldPinfo != null)
            {
                sb.Append("Old older position#3 = '");
                sb.Append(oldPinfo.OlderPositionNumber3);
                sb.Append("', ");
                sb.Append("old older position#2 = '");
                sb.Append(oldPinfo.OlderPositionNumber2);
                sb.Append("', ");
                sb.Append("old older position#1 = '");
                sb.Append(oldPinfo.OlderPositionNumber1);
                sb.Append("', ");
                sb.Append("old school# = '");
                sb.Append(oldPinfo.SchNumber);
                sb.Append("', ");
                sb.Append("old position type code = '");
                sb.Append(oldPinfo.PositionTypeCode);
                sb.Append("', ");
                sb.Append("old employee type code = '");
                sb.Append(oldPinfo.EmployeeTypeCode);
                sb.Append("', ");
                sb.Append("old position enddate = '");
                sb.Append(oldPinfo.PositionEndDate);
                sb.Append("', ");
                sb.Append("old Position FTE  = '");
                sb.Append(oldPinfo.PositionFTE);
                sb.Append("', ");
                sb.Append("old occupation type = '");
                sb.Append(oldPinfo.OccupationType);
                sb.Append("'. ");
                sb.Append("old notes = '");
                sb.Append(oldNote);
                sb.Append("'. ");
            }

            if (newPinfo != null)
            {
                sb.Append("New older position#3 = '");
                sb.Append(newPinfo.OlderPositionNumber3);
                sb.Append("', ");
                sb.Append("new older position#2 = '");
                sb.Append(newPinfo.OlderPositionNumber2);
                sb.Append("', ");
                sb.Append("new older position#1 = '");
                sb.Append(newPinfo.OlderPositionNumber1);
                sb.Append("', ");
                sb.Append("new school# = '");
                sb.Append(newPinfo.SchNumber);
                sb.Append("', ");
                sb.Append("new position type code = '");
                sb.Append(newPinfo.PositionTypeCode);
                sb.Append("', ");
                sb.Append("new employee type code = '");
                sb.Append(newPinfo.EmployeeTypeCode);
                sb.Append("', ");
                sb.Append("new position enddate = '");
                sb.Append(newPinfo.PositionEndDate);
                sb.Append("', ");
                sb.Append("new Position FTE  = '");
                sb.Append(newPinfo.PositionFTE);
                sb.Append("', ");
                sb.Append("new occupation type = '");
                sb.Append(newPinfo.OccupationType);
                sb.Append("'. ");
                sb.Append("new notes = '");
                sb.Append(newNote);
                sb.Append("'. ");
            }
            return sb;
        }


        public StringBuilder GetCostCentreChanges(CostCentreDetail oldC, CostCentreDetail newC)
        {
            StringBuilder sb = new StringBuilder();

            if (oldC != null)
            {
                sb.Append("Old Cost Centre code = '");
                sb.Append(oldC.CostCentre);
                sb.Append("', ");
                sb.Append("old Fund # = '");
                sb.Append(oldC.Fund);
                sb.Append("', ");
                sb.Append("old Payroll WBS = '");
                sb.Append(oldC.PayrollWBS);
                sb.Append("', ");
                sb.Append("old RCC JDE Payroll code = '");
                sb.Append(oldC.RCCJDEPayrollCode);
                sb.Append("', ");
                sb.Append("old GL account = '");
                sb.Append(oldC.GLAccount);
                sb.Append("'. ");
                
            }

            if (newC != null)
            {
                sb.Append("New Cost Centre code = '");
                sb.Append(newC.CostCentre);
                sb.Append("', ");
                sb.Append("new Fund # = '");
                sb.Append(newC.Fund);
                sb.Append("', ");
                sb.Append("new Payroll WBS = '");
                sb.Append(newC.PayrollWBS);
                sb.Append("', ");
                sb.Append("new RCC JDE Payroll code = '");
                sb.Append(newC.RCCJDEPayrollCode);
                sb.Append("', ");
                sb.Append("new GL account = '");
                sb.Append(newC.GLAccount);
                sb.Append("'. ");
            }
            return sb;
        }
    }
}




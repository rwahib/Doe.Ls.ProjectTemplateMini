

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
    public partial class RolePositionDescriptionHistoryRepository : BaseRepository<RolePositionDescriptionHistory> 
    {
        public RolePositionDescriptionHistoryRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<RolePositionDescriptionHistory> List()
        {                       
            return base.List()
                    .Include(ent=>ent.RolePositionDescription) 
                    .OrderByDescending(ent=>ent.CreatedDate);
        }

        public override void Insert(RolePositionDescriptionHistory entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(RolePositionDescriptionHistory entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<RolePositionDescriptionHistory> FilterRolePositionDescriptionHistorys(IQueryable<RolePositionDescriptionHistory> rolePositionDescriptionHistorys, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredRolePositionDescriptionHistory = rolePositionDescriptionHistorys.Where(ent => 
                    ent.RolePositionDescriptionHistoryId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.RolePositionDescription.Title) && ent.RolePositionDescription.Title.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Action) && ent.Action.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.StatusFrom) && ent.StatusFrom.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.StatusTo) && ent.StatusTo.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.AdditionalInfo) && ent.AdditionalInfo.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredRolePositionDescriptionHistory.OrderBy(e => e.RolePositionDescriptionHistoryId);
        }



        public void LogHistoryOnCreateRolePositioinDesc(RolePositionDescription rolePositionDesc, string userName)
        {
            var history = new RolePositionDescriptionHistory
            {
                RolePositionDescId = rolePositionDesc.RolePositionDescId,
                Action = Enum.GetName(typeof(Enums.ActionType), Enums.ActionType.Create),
                StatusFrom = "NA",

                StatusTo = Enum.GetName(typeof(Enums.StatusValue), rolePositionDesc.StatusId),
                AdditionalInfo = rolePositionDesc.IsPositionDescription ? 
                "Position description was created." : "Role description was created.",
                CreatedBy = userName,
                CreatedDate = DateTime.Now
            };

            Insert(history);
        }

        public void LogHistoryOnClone(RolePositionDescription rolePositionDesc, string sourceDocNum, string userName)
        {
            var history = new RolePositionDescriptionHistory
            {
                RolePositionDescId = rolePositionDesc.RolePositionDescId,
                Action = Enum.GetName(typeof(Enums.ActionType), Enums.ActionType.Create),
                StatusFrom = "New",

                StatusTo = Enum.GetName(typeof(Enums.StatusValue), rolePositionDesc.StatusId),
                AdditionalInfo = rolePositionDesc.IsPositionDescription ?
                "Position description was cloned from " + sourceDocNum + "." : 
                "Role description was cloned from " + sourceDocNum + ".",
                CreatedBy = userName,
                CreatedDate = DateTime.Now
            };

            Insert(history);
        }


        public void LogHistoryWhenUpdated(int rolePosDescId, int oldStatusId, int newStatusId, StringBuilder sb ,string sectionName, string userName)
        {
            //add to history
            var history = new RolePositionDescriptionHistory
            {
                RolePositionDescId = rolePosDescId,
                Action = Enum.GetName(typeof(Enums.ActionType), Enums.ActionType.Update),
                StatusFrom = Enum.GetName(typeof(Enums.StatusValue), oldStatusId),
                StatusTo = Enum.GetName(typeof(Enums.StatusValue), newStatusId),
                AdditionalInfo = sectionName + " has been updated. " + sb.ToString(),
                CreatedBy = userName,
                CreatedDate = DateTime.Now
            };

            Insert(history);
        }


        public StringBuilder  GetBudgetChanges(RoleDescription oldRd,RoleDescription newRd)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Old Budget/Expenditure = '");
            sb.Append(oldRd.BudgetExpenditure);
            sb.Append("', old Budget/Expenditure value = '");
            sb.Append(oldRd.BudgetExpenditureValue);
            sb.Append("', old Budget extra notes = '");
            sb.Append(oldRd.BudgetExtraNotes);
            sb.Append("'. ");
            sb.Append("New Budget/Expenditure = '");
            sb.Append(newRd.BudgetExpenditure);
            sb.Append("', new Budget/Expenditure value = '");
            sb.Append(newRd.BudgetExpenditureValue);
            sb.Append("', new Budget extra notes = '");
            sb.Append(newRd.BudgetExtraNotes);
            sb.Append("'.");

            return sb;
         }


        public StringBuilder GetKeyAccountabilityChallengeChanges(RoleDescription oldRd, RoleDescription newRd)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Old key accountabilities = '");
            sb.Append(oldRd.KeyAccountabilities);
            sb.Append("'. Old key challenges = '");
            sb.Append(oldRd.KeyChallenges);
            sb.Append("'. ");
            sb.Append("New key accountabilities = '");
            sb.Append(newRd.KeyAccountabilities);
            sb.Append("'. New key challenges = '");
            sb.Append(newRd.KeyChallenges);
            sb.Append("'");

            return sb;
        }

        public StringBuilder GetEssentialReqChanges(RoleDescription oldRd, RoleDescription newRd)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Old EssentialRequirements = '");
            sb.Append(oldRd.EssentialRequirements);
            sb.Append("'. ");
            sb.Append("New EssentialRequirements = '");
            sb.Append(newRd.EssentialRequirements);
            sb.Append("'");

            return sb;
        }

        public StringBuilder GetBasicDetailsChanges(RoleDescription oldRd, RolePositionDescription oldRpd, 
            RoleDescription newRd, RolePositionDescription newRpd)
        {
            if (oldRd == null)
            {
                oldRd = new RoleDescription
                {
                    RolePrimaryPurpose = string.Empty,
                    DecisionMaking = string.Empty,
                    ANZSCOCode = string.Empty,
                    PCATCode = string.Empty,
                    SeniorExecutiveWorkLevelStandards = string.Empty
                };
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Old Doc# = '");
            sb.Append(oldRpd.DocNumber);
            sb.Append("', ");
            sb.Append("old Title = '");
            sb.Append(oldRpd.Title);
            sb.Append("', ");
            sb.Append("old Grade = '");
            sb.Append(oldRpd.GradeCode);
            sb.Append("', ");
            sb.Append("old RolePrimaryPurpose = '");
            sb.Append(oldRd.RolePrimaryPurpose);
            sb.Append("', ");
            sb.Append("old DecisionMaking = '");
            sb.Append(oldRd.DecisionMaking);
            sb.Append("', ");
            sb.Append("old ANZSCOCode = '");
            sb.Append(oldRd.ANZSCOCode);
            sb.Append("', ");
            sb.Append("old PCATCode = '");
            sb.Append(oldRd.PCATCode);
            sb.Append("', ");
            sb.Append("old SeniorExecutiveWorkLevelStandards = '");
            sb.Append(oldRd.SeniorExecutiveWorkLevelStandards);
            sb.Append("'. ");

            sb.Append("New doc# = '");
            sb.Append(newRpd.DocNumber);
            sb.Append("'. ");
            sb.Append("new title = '");
            sb.Append(newRpd.Title);
            sb.Append("'. ");
            sb.Append("new grade = '");
            sb.Append(newRpd.GradeCode);
            sb.Append("',");
            sb.Append("new RolePrimaryPurpose = '");
            sb.Append(newRd.RolePrimaryPurpose);
            sb.Append("', ");
            sb.Append("new DecisionMaking = '");
            sb.Append(newRd.DecisionMaking);
            sb.Append("', ");
            sb.Append("new ANZSCOCode = '");
            sb.Append(newRd.ANZSCOCode);
            sb.Append("', ");
            sb.Append("new PCATCode = '");
            sb.Append(newRd.PCATCode);
            sb.Append("', ");
            sb.Append("new SeniorExecutiveWorkLevelStandards = '");
            sb.Append(newRd.SeniorExecutiveWorkLevelStandards);
            sb.Append("'. ");

            return sb;
        }


        public StringBuilder GetBreifStatementDutiesChanges(PositionDescription oldPd, PositionDescription newPd)
        {

            if (oldPd == null)
            {
                oldPd = new PositionDescription()
                {
                    BriefRoleStatement = string.Empty,
                    StatementOfDuties = string.Empty
                  
                };
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Old BriefRoleStatement = '");
            sb.Append(oldPd.BriefRoleStatement);
            sb.Append("', ");
            sb.Append("old StatementOfDuties = '");
            sb.Append(oldPd.StatementOfDuties);
            sb.Append("'. ");

            sb.Append("New BriefRoleStatement = '");
            sb.Append(newPd.BriefRoleStatement);
            sb.Append("', ");
            sb.Append("new StatementOfDuties = '");
            sb.Append(newPd.StatementOfDuties);
            sb.Append("'.");

            return sb;
        }

    }

}





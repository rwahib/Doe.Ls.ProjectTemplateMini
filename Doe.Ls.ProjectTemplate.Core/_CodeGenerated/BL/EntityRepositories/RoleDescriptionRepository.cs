 


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
    public partial class RoleDescriptionRepository : BaseRepository<RoleDescription> 
    {
        public RoleDescriptionRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<RoleDescription> List()
        {                       
            return base.List()
                    .Include(ent=>ent.KeyRelationships) 
                    .Include(ent=>ent.RoleCapabilities) 
                    .Include(ent=>ent.RolePositionDescription) 
                    .OrderBy(ent=>ent.RoleDescriptionId);
        }

        public override void Insert(RoleDescription entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(RoleDescription entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<RoleDescription> FilterRoleDescriptions(IQueryable<RoleDescription> roleDescriptions, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredRoleDescription = roleDescriptions.Where(ent => 
                    (!string.IsNullOrEmpty(ent.RolePositionDescription.Title) && ent.RolePositionDescription.Title.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Cluster) && ent.Cluster.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.SeniorExecutiveWorkLevelStandards) && ent.SeniorExecutiveWorkLevelStandards.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.ANZSCOCode) && ent.ANZSCOCode.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.PCATCode) && ent.PCATCode.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.AgencyOverview) && ent.AgencyOverview.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Agency) && ent.Agency.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.AgencyWebsite) && ent.AgencyWebsite.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.RolePrimaryPurpose) && ent.RolePrimaryPurpose.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.KeyAccountabilities) && ent.KeyAccountabilities.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.KeyChallenges) && ent.KeyChallenges.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.DecisionMaking) && ent.DecisionMaking.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.ReportingLine) && ent.ReportingLine.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.DirectReports) && ent.DirectReports.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.BudgetExpenditure) && ent.BudgetExpenditure.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.BudgetExpenditureValue) && ent.BudgetExpenditureValue.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.BudgetExtraNotes) && ent.BudgetExtraNotes.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.EssentialRequirements) && ent.EssentialRequirements.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.RoleCapabilityItems) && ent.RoleCapabilityItems.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CapabilitySummary) && ent.CapabilitySummary.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.FocusCapabilities) && ent.FocusCapabilities.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.VersionStatus) && ent.VersionStatus.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.OldPDFileName) && ent.OldPDFileName.ToLower().Contains(searchWord))
);

            return filteredRoleDescription.OrderBy(e => e.RoleDescriptionId);
        }
    }
}




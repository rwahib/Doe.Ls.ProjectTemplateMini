

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
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories 
{
    public partial class CapabilityLevelRepository : BaseRepository<CapabilityLevel> 
    {
        public CapabilityLevelRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<CapabilityLevel> List()
        {                       
            return base.List()
                    .Include(ent=>ent.CapabilityBehaviourIndicators) 
                    .Include(ent=>ent.RoleCapabilities) 
                    .OrderBy(ent=>ent.CapabilityLevelId);
        }

        public override void Insert(CapabilityLevel entity)
        {
            entity.DateCreated = DateTime.Now;
            entity.LastUpdated = DateTime.Now;
            if (string.IsNullOrWhiteSpace(entity.CreatedBy))
            {
                entity.CreatedBy = SessionService.GetCurrentUser().UserName;
            }
            if (string.IsNullOrWhiteSpace(entity.LastModifiedBy))
            {
                entity.LastModifiedBy = SessionService.GetCurrentUser().UserName;
            }


            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(CapabilityLevel entity, bool refresh = true) 
        {
            entity.DateCreated = DateTime.Now;
            entity.LastUpdated = DateTime.Now;
            if (string.IsNullOrWhiteSpace(entity.CreatedBy))
            {
                entity.CreatedBy = SessionService.GetCurrentUser().UserName;
            }
            if (string.IsNullOrWhiteSpace(entity.LastModifiedBy))
            {
                entity.LastModifiedBy = SessionService.GetCurrentUser().UserName;
            }

            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<CapabilityLevel> FilterCapabilityLevels(IQueryable<CapabilityLevel> capabilityLevels, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredCapabilityLevel = capabilityLevels.Where(ent => 
                    ent.CapabilityLevelId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.LevelName) && ent.LevelName.ToLower().Contains(searchWord))
                    || ent.DisplayOrder.ToString().Contains(searchWord)
                    || ent.LevelOrder.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredCapabilityLevel.OrderBy(e => e.CapabilityLevelId);
        }

        public List<CapabilityLevel> LoadCapabilityLevelsForRoleDesc(RoleDescCapabilityMatrix matrix)
        {

            var capabilityLevelItems = List().ToList();
            var name = string.Empty;

            if (matrix != null && matrix.HighlyAdvanced_Min == 0 && matrix.HighlyAdvanced_Max == 0)
            {
                name = Enum.GetName(typeof(Enums.CapablityLevel), 5);
                capabilityLevelItems.RemoveAll(r => r.LevelName.Replace(" ", "") == name);
            }

            if (matrix != null && matrix.Advanced_Min == 0 && matrix.Advanced_Max == 0)
            {
                name = Enum.GetName(typeof(Enums.CapablityLevel), 3);
                capabilityLevelItems.RemoveAll(r => r.LevelName == name);
            }

            if (matrix != null && matrix.Adept_Min == 0 && matrix.Adept_Max == 0)
            {
                name = Enum.GetName(typeof(Enums.CapablityLevel), 1);
                capabilityLevelItems.RemoveAll(r => r.LevelName == name);
            }

            if (matrix != null && matrix.Intermediate_Min == 0 && matrix.Intermediate_Max == 0)
            {
                name = Enum.GetName(typeof(Enums.CapablityLevel), 2);
                capabilityLevelItems.RemoveAll(r => r.LevelName == name);
            }

            if (matrix != null && matrix.Foundational_Min == 0 && matrix.Foundational_Max == 0)
            {
                name = Enum.GetName(typeof(Enums.CapablityLevel), 4);
                capabilityLevelItems.RemoveAll(r => r.LevelName == name);
            }

            return capabilityLevelItems;
          }
    }
}




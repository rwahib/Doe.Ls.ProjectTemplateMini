

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
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Data;


namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories 
{
    public partial class RoleCapabilityRepository : BaseRepository<RoleCapability> 
    {
        public RoleCapabilityRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public virtual void AddRange(IEnumerable<RoleCapability> entities)
        {
            _dbSet.AddRange(entities);
            SaveChanges();
        }
        public override IQueryable<RoleCapability> List()
        {                       
            return base.List()
                    .Include(ent=>ent.CapabilityLevel) 
                    .Include(ent=>ent.CapabilityName) 
                    .Include(ent=>ent.RoleDescription) 
                    .OrderBy(ent=>ent.RoleDescriptionId);
        }

        public  IQueryable<RoleCapability> BaseList()
        {
            return base.List();
        }

        public override void Insert(RoleCapability entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(RoleCapability entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<RoleCapability> FilterRoleCapabilitys(IQueryable<RoleCapability> roleCapabilitys, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredRoleCapability = roleCapabilitys.Where(ent => 
                    (!string.IsNullOrEmpty(ent.RoleDescription.OldPDFileName) && ent.RoleDescription.OldPDFileName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CapabilityName.Name) && ent.CapabilityName.Name.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CapabilityLevel.LevelName) && ent.CapabilityLevel.LevelName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredRoleCapability.OrderBy(e => e.RoleDescriptionId);
        }


        public List<CapabilityGroupLight> GenerateCapabilityModel(int roleDescriptionId)
        {
            var rolecapList = this.List().Where(l => l.RoleDescriptionId == roleDescriptionId).ToList();
            var nameIdList = rolecapList.Select(rc => rc.CapabilityNameId);
            var capabilityGroupItems = RepositoryFactory.GetService<CapabilityGroupRepository>().List().ToList();
            var capabilityGroupList = new List<CapabilityGroupLight>();
            foreach (var cg in capabilityGroupItems)
            {
                var capGrp = new CapabilityGroupLight();
                var cnameList = new List<CapabilityNameLight>();
                capGrp.GroupName = cg.GroupName;
                capGrp.CapabilityGroupId = cg.CapabilityGroupId;
                foreach (var cn in cg.CapabilityNames)
                {
                    var cname = new CapabilityNameLight();
                    cname.CapabilityNameId = cn.CapabilityNameId;
                    cname.Name = cn.Name;

                    if (nameIdList.Contains(cn.CapabilityNameId))
                    {
                        var roleCap = rolecapList.FirstOrDefault(rc => rc.CapabilityNameId == cn.CapabilityNameId);
                        cname.LevelId = roleCap.CapabilityLevelId;
                        cname.Highlighted = roleCap.Highlighted;
                        cname.Selected = true;
                        var indcators = roleCap.CapabilityName.CapabilityBehaviourIndicators;
                        if (indcators != null)
                        {
                            var indContext = indcators.FirstOrDefault(
                                cb =>
                                    cb.CapabilityLevelId == roleCap.CapabilityLevelId &&
                                    cb.CapabilityNameId == roleCap.CapabilityNameId);
                            cname.IndContext = indContext != null ? indContext.IndicatorContext : "";
                        }
                        else
                        {
                            cname.IndContext = "";
                        }
                    }
                    cnameList.Add(cname);
                }
                capGrp.CapabilityNames = cnameList.OrderBy(c => c.Name).ToList();
                capabilityGroupList.Add(capGrp);
            }
            return capabilityGroupList;
        }


        public void UpdateRoleCapabilities(int intRoleDescriptionId, List<RoleCapability> rolecapabilityList)
        {
            var oldValues = this.List().Where(l => l.RoleDescriptionId == intRoleDescriptionId).ToList();
            
            this.RemoveRange(oldValues);
            this.AddRange(rolecapabilityList);
        }

        public void BulkInsertRoleCapabilities(List<RoleCapability> rolecapabilityList)
        {
            this.AddRange(rolecapabilityList);
        }

        [Unity.Attributes.Dependency]
        public RoleDescriptionRepository RoleDescriptionRepository { get; set; }

       

    }
}




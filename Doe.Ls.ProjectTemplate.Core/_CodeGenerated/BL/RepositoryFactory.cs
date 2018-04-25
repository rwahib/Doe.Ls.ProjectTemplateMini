 



using System.Web;
using Doe.Ls.EntityBase;
using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.Logging;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Data;


namespace Doe.Ls.ProjectTemplate.Core.BL 
{
    public class RepositoryFactory : RepositoryFactoryApplicationBase {
       
        public RepositoryFactory(UnityContainer container)
            : base(container) {
        }
        public RepositoryFactory() { }

        public override void RegisterEntityRepositories() 
        {

            #region Entity services registration

            var unitOfWork=new UnitOfWork();
            unitOfWork.DbContext.Configuration.LazyLoadingEnabled = false;
            Container.RegisterInstance<IUnitOfWork>(unitOfWork);
            
            Container.RegisterType<IRepository<AppEntityType>, AppEntityTypeRepository>();
            Container.RegisterType<IRepository<AppObjectInfo>, AppObjectInfoRepository>();
            Container.RegisterType<IRepository<BusinessUnit>, BusinessUnitRepository>();
            Container.RegisterType<IRepository<CapabilityBehaviourIndicator>, CapabilityBehaviourIndicatorRepository>();
            Container.RegisterType<IRepository<CapabilityGroup>, CapabilityGroupRepository>();
            Container.RegisterType<IRepository<CapabilityLevel>, CapabilityLevelRepository>();
            Container.RegisterType<IRepository<CapabilityName>, CapabilityNameRepository>();
            Container.RegisterType<IRepository<CostCentreDetail>, CostCentreDetailRepository>();
            Container.RegisterType<IRepository<Directorate>, DirectorateRepository>();
            Container.RegisterType<IRepository<Employee>, EmployeeRepository>();
            Container.RegisterType<IRepository<EmployeePosition>, EmployeePositionRepository>();
            Container.RegisterType<IRepository<EmployeeType>, EmployeeTypeRepository>();
            Container.RegisterType<IRepository<Executive>, ExecutiveRepository>();
            Container.RegisterType<IRepository<Focus>, FocusRepository>();
            Container.RegisterType<IRepository<FunctionalArea>, FunctionalAreaRepository>();
            Container.RegisterType<IRepository<GeneralLog>, GeneralLogRepository>();
            Container.RegisterType<IRepository<GlobalItem>, GlobalItemRepository>();
            Container.RegisterType<IRepository<GlobalSetting>, GlobalSettingRepository>();
            Container.RegisterType<IRepository<Grade>, GradeRepository>();
            Container.RegisterType<IRepository<HierarchyLevel>, HierarchyLevelRepository>();
            Container.RegisterType<IRepository<KeyRelationship>, KeyRelationshipRepository>();
            Container.RegisterType<IRepository<Location>, LocationRepository>();
            Container.RegisterType<IRepository<LookupFocusGradeCriteria>, LookupFocusGradeCriteriaRepository>();
            Container.RegisterType<IRepository<OccupationType>, OccupationTypeRepository>();
            Container.RegisterType<IRepository<OrgLevel>, OrgLevelRepository>();
            Container.RegisterType<IRepository<PositionDescription>, PositionDescriptionRepository>();
            Container.RegisterType<IRepository<PositionFocusCriteria>, PositionFocusCriteriaRepository>();
            Container.RegisterType<IRepository<PositionHistory>, PositionHistoryRepository>();
            Container.RegisterType<IRepository<PositionInformation>, PositionInformationRepository>();
            Container.RegisterType<IRepository<PositionLevel>, PositionLevelRepository>();
            Container.RegisterType<IRepository<PositionNote>, PositionNoteRepository>();
            Container.RegisterType<IRepository<PositionStatusValue>, PositionStatusValueRepository>();
            Container.RegisterType<IRepository<PositionType>, PositionTypeRepository>();
            Container.RegisterType<IRepository<RelationshipScope>, RelationshipScopeRepository>();
            Container.RegisterType<IRepository<RoleCapability>, RoleCapabilityRepository>();
            Container.RegisterType<IRepository<RoleDescCapabilityMatrix>, RoleDescCapabilityMatrixRepository>();
            Container.RegisterType<IRepository<RoleDescription>, RoleDescriptionRepository>();
            Container.RegisterType<IRepository<RolePositionDescription>, RolePositionDescriptionRepository>();
            Container.RegisterType<IRepository<RolePositionDescriptionHistory>, RolePositionDescriptionHistoryRepository>();
            Container.RegisterType<IRepository<ScriptHistory>, ScriptHistoryRepository>();
            Container.RegisterType<IRepository<SelectionCriteria>, SelectionCriteriaRepository>();
            Container.RegisterType<IRepository<StatusValue>, StatusValueRepository>();
            Container.RegisterType<IRepository<SysMessage>, SysMessageRepository>();
            Container.RegisterType<IRepository<SysMsgCategory>, SysMsgCategoryRepository>();
            Container.RegisterType<IRepository<SysRole>, SysRoleRepository>();
            Container.RegisterType<IRepository<SysUser>, SysUserRepository>();
            Container.RegisterType<IRepository<SysUserRole>, SysUserRoleRepository>();
            Container.RegisterType<IRepository<TeamType>, TeamTypeRepository>();
            Container.RegisterType<IRepository<TrimRecord>, TrimRecordRepository>();
            Container.RegisterType<IRepository<Unit>, UnitRepository>();
            Container.RegisterType<IRepository<WfAction>, WfActionRepository>();
            Container.RegisterType<IRepository<Position>, PositionRepository>();

            #endregion

        }       
    }
}



using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Data;

using Unity.Resolution;

namespace Doe.Ls.ProjectTemplate.Core.BL
{
    public partial class ServiceRepository
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public IRepositoryFactory RepositoryFactory
        {
            get { return _repositoryFactory; }
        }

        public ServiceRepository(IRepositoryFactory factory)
        {
            _repositoryFactory = factory;
        }


        #region Entity services
        public IUnitOfWork GetUnitOfWork(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IUnitOfWork>();
        }

      
        public AppEntityTypeRepository AppEntityTypeRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<AppEntityType>>(overrides) as AppEntityTypeRepository;
        }


        public AppObjectInfoRepository AppObjectInfoRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<AppObjectInfo>>(overrides) as AppObjectInfoRepository;
        }


        public BusinessUnitRepository BusinessUnitRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<BusinessUnit>>(overrides) as BusinessUnitRepository;
        }


        public CapabilityBehaviourIndicatorRepository CapabilityBehaviourIndicatorRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<CapabilityBehaviourIndicator>>(overrides) as CapabilityBehaviourIndicatorRepository;
        }

        public T GetService<T>(params ResolverOverride[] overrides) where T:class
            {
            return _repositoryFactory.GetService<T>(overrides) as T;
            }

        public CapabilityGroupRepository CapabilityGroupRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<CapabilityGroup>>(overrides) as CapabilityGroupRepository;
        }


        public CapabilityLevelRepository CapabilityLevelRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<CapabilityLevel>>(overrides) as CapabilityLevelRepository;
        }


        public CapabilityNameRepository CapabilityNameRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<CapabilityName>>(overrides) as CapabilityNameRepository;
        }


        public CostCentreDetailRepository CostCentreDetailRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<CostCentreDetail>>(overrides) as CostCentreDetailRepository;
        }


        public DirectorateRepository DirectorateRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<Directorate>>(overrides) as DirectorateRepository;
        }


        public EmployeeRepository EmployeeRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<Employee>>(overrides) as EmployeeRepository;
        }


        public EmployeePositionRepository EmployeePositionRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<EmployeePosition>>(overrides) as EmployeePositionRepository;
        }


        public EmployeeTypeRepository EmployeeTypeRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<EmployeeType>>(overrides) as EmployeeTypeRepository;
        }


        public ExecutiveRepository ExecutiveRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<Executive>>(overrides) as ExecutiveRepository;
        }


        public FocusRepository FocusRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<Focus>>(overrides) as FocusRepository;
        }


        public FunctionalAreaRepository FunctionalAreaRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<FunctionalArea>>(overrides) as FunctionalAreaRepository;
        }


        public GeneralLogRepository GeneralLogRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<GeneralLog>>(overrides) as GeneralLogRepository;
        }


        public GlobalItemRepository GlobalItemRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<GlobalItem>>(overrides) as GlobalItemRepository;
        }


        public GradeRepository GradeRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<Grade>>(overrides) as GradeRepository;
        }


        public HierarchyLevelRepository HierarchyLevelRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<HierarchyLevel>>(overrides) as HierarchyLevelRepository;
        }


        public KeyRelationshipRepository KeyRelationshipRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<KeyRelationship>>(overrides) as KeyRelationshipRepository;
        }


        public LocationRepository LocationRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<Location>>(overrides) as LocationRepository;
        }


        public LookupFocusGradeCriteriaRepository LookupFocusGradeCriteriaRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<LookupFocusGradeCriteria>>(overrides) as LookupFocusGradeCriteriaRepository;
        }


        public OccupationTypeRepository OccupationTypeRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<OccupationType>>(overrides) as OccupationTypeRepository;
        }


        public OrgLevelRepository OrgLevelRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<OrgLevel>>(overrides) as OrgLevelRepository;
        }


        public PositionRepository PositionRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<Position>>(overrides) as PositionRepository;
        }


        public PositionDescriptionRepository PositionDescriptionRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<PositionDescription>>(overrides) as PositionDescriptionRepository;
        }


        public PositionFocusCriteriaRepository PositionFocusCriteriaRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<PositionFocusCriteria>>(overrides) as PositionFocusCriteriaRepository;
        }


        public PositionInformationRepository PositionInformationRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<PositionInformation>>(overrides) as PositionInformationRepository;
        }


        public PositionLevelRepository PositionLevelRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<PositionLevel>>(overrides) as PositionLevelRepository;
        }


        public PositionNoteRepository PositionNoteRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<PositionNote>>(overrides) as PositionNoteRepository;
        }


        public PositionStatusValueRepository PositionStatusValueRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<PositionStatusValue>>(overrides) as PositionStatusValueRepository;
        }


        public PositionTypeRepository PositionTypeRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<PositionType>>(overrides) as PositionTypeRepository;
        }


        public RelationshipScopeRepository RelationshipScopeRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<RelationshipScope>>(overrides) as RelationshipScopeRepository;
        }


        public RoleCapabilityRepository RoleCapabilityRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<RoleCapability>>(overrides) as RoleCapabilityRepository;
        }


        public RoleDescCapabilityMatrixRepository RoleDescCapabilityMatrixRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<RoleDescCapabilityMatrix>>(overrides) as RoleDescCapabilityMatrixRepository;
        }


        public RoleDescriptionRepository RoleDescriptionRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<RoleDescription>>(overrides) as RoleDescriptionRepository;
        }


        public RolePositionDescriptionRepository RolePositionDescriptionRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<RolePositionDescription>>(overrides) as RolePositionDescriptionRepository;
        }


        public SelectionCriteriaRepository SelectionCriteriaRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<SelectionCriteria>>(overrides) as SelectionCriteriaRepository;
        }


        public StatusValueRepository StatusValueRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<StatusValue>>(overrides) as StatusValueRepository;
        }


        public SysRoleRepository SysRoleRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<SysRole>>(overrides) as SysRoleRepository;
        }


        public SysUserRepository SysUserRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<SysUser>>(overrides) as SysUserRepository;
        }


        public SysUserRoleRepository SysUserRoleRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<SysUserRole>>(overrides) as SysUserRoleRepository;
        }


        public TeamTypeRepository TeamTypeRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<TeamType>>(overrides) as TeamTypeRepository;
        }


        public UnitRepository UnitRepository(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IRepository<Unit>>(overrides) as UnitRepository;
        }
        public PositionHistoryRepository PositionHistoryRepository(params ResolverOverride[] overrides)
            {
            return _repositoryFactory.GetService<IRepository<PositionHistory>>(overrides) as PositionHistoryRepository;
            }


        public RolePositionDescriptionHistoryRepository RolePositionDescriptionHistoryRepository(params ResolverOverride[] overrides)
            {
            return _repositoryFactory.GetService<IRepository<RolePositionDescriptionHistory>>(overrides) as RolePositionDescriptionHistoryRepository;
            }


        public WfActionRepository WfActionRepository(params ResolverOverride[] overrides)
            {
            return _repositoryFactory.GetService<IRepository<WfAction>>(overrides) as WfActionRepository;
            }

        public SysMessageRepository SysMessageRepository(params ResolverOverride[] overrides)
            {
            return _repositoryFactory.GetService<IRepository<SysMessage>>(overrides) as SysMessageRepository;
            }


        public SysMsgCategoryRepository SysMsgCategoryRepository(params ResolverOverride[] overrides)
            {
            return _repositoryFactory.GetService<IRepository<SysMsgCategory>>(overrides) as SysMsgCategoryRepository;
            }

        public TrimRecordRepository TrimRecordRepository(params ResolverOverride[] overrides) {
            return _repositoryFactory.GetService<IRepository<TrimRecord>>(overrides) as TrimRecordRepository;
            }

        #endregion
        }
}
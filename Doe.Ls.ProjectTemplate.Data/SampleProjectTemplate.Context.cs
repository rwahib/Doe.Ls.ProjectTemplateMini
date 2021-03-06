﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Doe.Ls.ProjectTemplate.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SampleProjectTemplateEntities : DbContext
    {
        public SampleProjectTemplateEntities()
            : base("name=SampleProjectTemplateEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AppEntityType> AppEntityTypes { get; set; }
        public virtual DbSet<AppObjectInfo> AppObjectInfoes { get; set; }
        public virtual DbSet<BusinessUnit> BusinessUnits { get; set; }
        public virtual DbSet<CapabilityBehaviourIndicator> CapabilityBehaviourIndicators { get; set; }
        public virtual DbSet<CapabilityGroup> CapabilityGroups { get; set; }
        public virtual DbSet<CapabilityLevel> CapabilityLevels { get; set; }
        public virtual DbSet<CapabilityName> CapabilityNames { get; set; }
        public virtual DbSet<CostCentreDetail> CostCentreDetails { get; set; }
        public virtual DbSet<Directorate> Directorates { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }
        public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }
        public virtual DbSet<Executive> Executives { get; set; }
        public virtual DbSet<Focus> Foci { get; set; }
        public virtual DbSet<FunctionalArea> FunctionalAreas { get; set; }
        public virtual DbSet<GeneralLog> GeneralLogs { get; set; }
        public virtual DbSet<GlobalItem> GlobalItems { get; set; }
        public virtual DbSet<GlobalSetting> GlobalSettings { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<HierarchyLevel> HierarchyLevels { get; set; }
        public virtual DbSet<KeyRelationship> KeyRelationships { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LookupFocusGradeCriteria> LookupFocusGradeCriterias { get; set; }
        public virtual DbSet<OccupationType> OccupationTypes { get; set; }
        public virtual DbSet<OrgLevel> OrgLevels { get; set; }
        public virtual DbSet<PositionDescription> PositionDescriptions { get; set; }
        public virtual DbSet<PositionFocusCriteria> PositionFocusCriterias { get; set; }
        public virtual DbSet<PositionHistory> PositionHistories { get; set; }
        public virtual DbSet<PositionInformation> PositionInformations { get; set; }
        public virtual DbSet<PositionLevel> PositionLevels { get; set; }
        public virtual DbSet<PositionNote> PositionNotes { get; set; }
        public virtual DbSet<PositionStatusValue> PositionStatusValues { get; set; }
        public virtual DbSet<PositionType> PositionTypes { get; set; }
        public virtual DbSet<RelationshipScope> RelationshipScopes { get; set; }
        public virtual DbSet<RoleCapability> RoleCapabilities { get; set; }
        public virtual DbSet<RoleDescCapabilityMatrix> RoleDescCapabilityMatrices { get; set; }
        public virtual DbSet<RoleDescription> RoleDescriptions { get; set; }
        public virtual DbSet<RolePositionDescription> RolePositionDescriptions { get; set; }
        public virtual DbSet<RolePositionDescriptionHistory> RolePositionDescriptionHistories { get; set; }
        public virtual DbSet<ScriptHistory> ScriptHistories { get; set; }
        public virtual DbSet<SelectionCriteria> SelectionCriterias { get; set; }
        public virtual DbSet<StatusValue> StatusValues { get; set; }
        public virtual DbSet<SysMessage> SysMessages { get; set; }
        public virtual DbSet<SysMsgCategory> SysMsgCategories { get; set; }
        public virtual DbSet<SysRole> SysRoles { get; set; }
        public virtual DbSet<SysUser> SysUsers { get; set; }
        public virtual DbSet<SysUserRole> SysUserRoles { get; set; }
        public virtual DbSet<TeamType> TeamTypes { get; set; }
        public virtual DbSet<TrimRecord> TrimRecords { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<WfAction> WfActions { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
    
        [DbFunction("SampleProjectTemplateEntities", "fnSplitString")]
        public virtual IQueryable<fnSplitString_Result> fnSplitString(string @string, string delimiter)
        {
            var stringParameter = @string != null ?
                new ObjectParameter("string", @string) :
                new ObjectParameter("string", typeof(string));
    
            var delimiterParameter = delimiter != null ?
                new ObjectParameter("delimiter", delimiter) :
                new ObjectParameter("delimiter", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<fnSplitString_Result>("[SampleProjectTemplateEntities].[fnSplitString](@string, @delimiter)", stringParameter, delimiterParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> BulkBringToDraftPositions(string docNumber)
        {
            var docNumberParameter = docNumber != null ?
                new ObjectParameter("DocNumber", docNumber) :
                new ObjectParameter("DocNumber", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("BulkBringToDraftPositions", docNumberParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> BulkMarkAsImportPositions(string docNumber)
        {
            var docNumberParameter = docNumber != null ?
                new ObjectParameter("DocNumber", docNumber) :
                new ObjectParameter("DocNumber", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("BulkMarkAsImportPositions", docNumberParameter);
        }
    
        public virtual ObjectResult<MovePositionNumberToTargetDocNumber_Result> MovePositionNumberToTargetDocNumber(string sourcePositionNumber, string targetDocNumber)
        {
            var sourcePositionNumberParameter = sourcePositionNumber != null ?
                new ObjectParameter("sourcePositionNumber", sourcePositionNumber) :
                new ObjectParameter("sourcePositionNumber", typeof(string));
    
            var targetDocNumberParameter = targetDocNumber != null ?
                new ObjectParameter("targetDocNumber", targetDocNumber) :
                new ObjectParameter("targetDocNumber", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MovePositionNumberToTargetDocNumber_Result>("MovePositionNumberToTargetDocNumber", sourcePositionNumberParameter, targetDocNumberParameter);
        }
    
        public virtual int udp_cleanUnitTestData(string unitTestToken)
        {
            var unitTestTokenParameter = unitTestToken != null ?
                new ObjectParameter("UnitTestToken", unitTestToken) :
                new ObjectParameter("UnitTestToken", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("udp_cleanUnitTestData", unitTestTokenParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> udpGetNewKey(string objectName, Nullable<int> increment)
        {
            var objectNameParameter = objectName != null ?
                new ObjectParameter("objectName", objectName) :
                new ObjectParameter("objectName", typeof(string));
    
            var incrementParameter = increment.HasValue ?
                new ObjectParameter("increment", increment) :
                new ObjectParameter("increment", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("udpGetNewKey", objectNameParameter, incrementParameter);
        }
    
        public virtual ObjectResult<string> UpdateAllPositionHierarchy()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("UpdateAllPositionHierarchy");
        }
    
        public virtual int UpdatePositionHierarchy(Nullable<int> posId)
        {
            var posIdParameter = posId.HasValue ?
                new ObjectParameter("PosId", posId) :
                new ObjectParameter("PosId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdatePositionHierarchy", posIdParameter);
        }
    
        public virtual ObjectResult<string> UpdateRolePosDescriptionTitleCascade(string docNumber, string title)
        {
            var docNumberParameter = docNumber != null ?
                new ObjectParameter("DocNumber", docNumber) :
                new ObjectParameter("DocNumber", typeof(string));
    
            var titleParameter = title != null ?
                new ObjectParameter("Title", title) :
                new ObjectParameter("Title", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("UpdateRolePosDescriptionTitleCascade", docNumberParameter, titleParameter);
        }
    }
}

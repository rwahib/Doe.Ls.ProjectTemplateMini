using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions
{
    public static class RoleDescriptionExtensions
    {
       public static IEnumerable<KeyRelationship> InternalKeyRelationships(this RoleDescription rd)
        {
            return rd.KeyRelationships.Where(kr => kr.ScopeId == (int) Enums.ScopeType.Internal);
        }
        public static IEnumerable<KeyRelationship> ExternalKeyRelationships(this RoleDescription rd)
        {
            return rd.KeyRelationships.Where(kr => kr.ScopeId == (int)Enums.ScopeType.External);
        }

        public static IEnumerable<KeyRelationship> MinisterialKeyRelationships(this RoleDescription rd)
        {
            return rd.KeyRelationships.Where(kr => kr.ScopeId == (int)Enums.ScopeType.Ministerial);
        }

        public static IEnumerable<RoleCapability> SortCapabilityGroup(this RoleDescription rd)
        {
            return rd.RoleCapabilities
                .Where(r => r.CapabilityName.CapabilityGroupId == r.CapabilityName.CapabilityGroup.CapabilityGroupId)
                .OrderBy(r => r.CapabilityName.CapabilityGroup.DisplayOrder);
        }


        public static bool IsValid(this RoleDescription rd)
        {
            return (PosRdPdFactory.Create(rd)).IsValid();
        }
        }
}
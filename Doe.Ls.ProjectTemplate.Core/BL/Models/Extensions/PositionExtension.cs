using System;
using System.Collections.Generic;
using Doe.Ls.ProjectTemplate.Data;
using System.Linq;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions
    {
    public static class PositionExtension
        {
        public static bool HasTheSameOrganisationalStructureForThisRole(this Position position, Enums.UserRole role, UserInfoExtension userInfo)
            {
            if(userInfo.HasAdminOrPowerRole())
                {
                return true;
                }

            switch(role)
                {
                case Enums.UserRole.BusinessUnitDataEntry:
                    if(userInfo.IsBusinessUnitDataEntry)
                        if(position.UnitId == Enums.Cnt.Na) return true;
                        {
                        var ids = userInfo.ActiveRoleOrgLevelList.Where(r => r.IsActive && r.RoleId == (int)Enums.UserRole.BusinessUnitDataEntry).Select(r => r.StructureId);
                        if(ids.Contains(position.Unit.BUnitId.ToString()))
                            {
                            return true;
                            }
                        }
                    break;
                case Enums.UserRole.BusinessUnitAuthor:
                    if(userInfo.IsBusinessUnitAuthor)
                        {
                        if(position.UnitId == Enums.Cnt.Na) return true;
                        var ids = userInfo.ActiveRoleOrgLevelList.Where(r => r.IsActive && r.RoleId == (int)Enums.UserRole.BusinessUnitAuthor).Select(r => r.StructureId);
                        if(ids.Contains(position.Unit.BUnitId.ToString()))
                            {
                            return true;
                            }
                        }
                    break;
                case Enums.UserRole.DirectorateDataEntry:

                    if(userInfo.IsDirectorateDataEntry)
                        {
                        if(position.UnitId == Enums.Cnt.Na) return true;
                        var ids = userInfo.ActiveRoleOrgLevelList.Where(r => r.IsActive && r.RoleId == (int)Enums.UserRole.DirectorateDataEntry).Select(r => r.StructureId);
                        if(ids.Contains(position.Unit.BusinessUnit.DirectorateId.ToString()))
                            {
                            return true;
                            }
                        }
                    break;
                case Enums.UserRole.DirectorateEndorser:
                    if(userInfo.IsDirectorateEndorser)
                        {
                        if(position.UnitId == Enums.Cnt.Na) return true;
                        var ids = userInfo.ActiveRoleOrgLevelList.Where(r => r.IsActive && r.RoleId == (int)Enums.UserRole.DirectorateEndorser).Select(r => r.StructureId);
                        if(ids.Contains(position.Unit.BusinessUnit.DirectorateId.ToString()))
                            {
                            return true;
                            }
                        }
                    break;
                case Enums.UserRole.DivisionEditor:
                    if(userInfo.IsDivisionEditor)
                        {
                        if(position.UnitId == Enums.Cnt.Na) return true;
                        var ids = userInfo.ActiveRoleOrgLevelList.Where(r => r.IsActive && r.RoleId == (int)Enums.UserRole.DivisionEditor).Select(r => r.StructureId);
                        if(ids.Contains(position.Unit.BusinessUnit.Directorate.ExecutiveCod))
                            {
                            return true;
                            }
                        }
                    break;
                case Enums.UserRole.DivisionApprover:
                    if(userInfo.IsDivisionApprover)
                        {
                        if(position.UnitId == Enums.Cnt.Na) return true;
                        var ids = userInfo.ActiveRoleOrgLevelList.Where(r => r.IsActive && r.RoleId == (int)Enums.UserRole.DivisionApprover).Select(r => r.StructureId);
                        if(ids.Contains(position.Unit.BusinessUnit.Directorate.ExecutiveCod))
                            {
                            return true;
                            }
                        }
                    break;

                }







            return false;
            }
        public static bool HasTheSameOrganisationalStructureForThisRole(this Position position, UserInfoExtension userInfo)
            {
            var userRoles = userInfo.ActiveRoleOrgLevelList.Select(r => r.UserRole);
            foreach(var userRole in userRoles)
                {
                if(HasTheSameOrganisationalStructureForThisRole(position, userRole, userInfo)) return true;
                }
            return false;
            }
        public static bool HasAny(this IEnumerable<Position> positions, params Enums.StatusValue[] statusList)
            {
            var statusIds = statusList.ToIntegerList();
            if(positions == null) return false;

            if(!positions.Any()) return false;
            return positions.Any(pos => statusIds.Contains(pos.StatusId));

            }
        public static bool HasNonDeletedAny(this IEnumerable<Position> positions)
            {
            if(positions == null) return false;
            positions = positions.ToArray();
            if(!positions.Any()) return false;
            return positions.Any(pos => pos.StatusId!=(int)Enums.StatusValue.Deleted);

            }
        public static bool HasAnyIncludeLive(this IEnumerable<Position> positions, params Enums.StatusValue[] statusList)
            {
            
            var statusIds = statusList.ToIntegerList();
            statusIds.AddRange(new List<int>() { (int)Enums.StatusValue.Approved, (int)Enums.StatusValue.Imported});
            if(positions == null) return false;

            if(!positions.Any()) return false;
            return positions.Any(pos => statusIds.Contains(pos.StatusId));

            }
      
        }
    }
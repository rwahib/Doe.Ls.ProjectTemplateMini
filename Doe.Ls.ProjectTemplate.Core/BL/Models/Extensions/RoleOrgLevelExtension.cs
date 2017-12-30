using System;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions
    {
    public static class RoleOrgLevelExtension
        {
        public static UserRoleModel ToUserOrgLevelObject(this SysUserRole sr, ServiceRepository repository)
            {
            var model = new UserRoleModel
                {
                RoleId = sr.RoleId,
                OrgLevelId = sr.OrgLevelId,
                OrgLevelName = sr.OrgLevel?.OrgLevelName,
                StructureId = sr.StructureId,
                LastModifiedDate = sr.LastModifiedDate,
                LastModifiedBy = sr.LastModifiedBy,
                ActiveFrom = sr.ActiveFrom,
                ActiveTo = sr.ActiveTo,
                IsActive = sr.IsActive(),
                UserId = sr.UserId,                
                Note = sr.Note,
                CreatedDate = sr.CreatedDate,
                CreatedBy = sr.CreatedBy

                };
            model.SetDefaults(sr.SysUser,repository);            
            return model;
            }
        
        }
    }
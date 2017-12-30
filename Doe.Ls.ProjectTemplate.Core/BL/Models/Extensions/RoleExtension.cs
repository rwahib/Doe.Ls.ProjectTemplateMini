using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions
{
    public static class RoleExtension
    {
        public static Enums.UserRole GetEnum(this SysRole role)
        {
            return (Enums.UserRole) role.RoleId;
        }

    }
}
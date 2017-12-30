using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions
{
    public static class StatusValueExtension
    {
        public static Enums.StatusValue GetEnum(this StatusValue status)
        {
            return (Enums.StatusValue)status.StatusId;
        }

        public static bool IsLive(this StatusValue status)
            {
            var enumVal=(Enums.StatusValue)status.StatusId;
            return enumVal == Enums.StatusValue.Imported || enumVal == Enums.StatusValue.Approved;
            }
        }
}
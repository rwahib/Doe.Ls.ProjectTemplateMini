using System.Web.Security;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI;

namespace Doe.Ls.ProjectTemplate.Web.Controllers.Domain
    {
    public class SysController : AppControllerBase
        {

        public string ExpireAllCache()
            {
            AppCacheHelper.Expire(Enums.CacheRegion.Messages);
            AppCacheHelper.Expire(Enums.CacheRegion.Position);
            AppCacheHelper.Expire(Enums.CacheRegion.Default);
            return "All cache expired successfully ";
            }

        public string ExpireMessageListCache()
            {
            AppCacheHelper.Expire(Enums.CacheRegion.Messages);

            return "Messages cache expired successfully ";

            }

        public string ExpirePositionChartCache()
            {
            AppCacheHelper.Expire(Enums.CacheRegion.Position);

            return "Positions cache expired successfully ";

            }

        public string ResetApplication()
            {
            System.Web.HttpRuntime.UnloadAppDomain();
            return "Application unloaded successfully ";
            }

        public string UpdateAllhierarchy()
        {
            var rep = this.ServiceRepository.PositionRepository();
            rep.UpdateAllPositionHierarchy();
            return "Hierarchy updated successfully ";
            }

        public string ExpireSession()
            {
            FormsAuthentication.SignOut();
            SessionService.Abandon();
            return "Session expired successfully ";
            }

        }
    }
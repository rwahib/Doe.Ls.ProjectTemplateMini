using System.Web;
using System.Web.Mvc;
using Doe.Ls.EntityBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes
{
    public abstract class PositionEstablishmentRoleAttribute : AuthorizeAttribute
    {
        protected bool AuthorizeCoreBase(HttpContextBase httpContext)
        {
            if(!httpContext.User.Identity.IsAuthenticated) return false;
            var user = httpContext.Session[Cnt.CurrentUserKey] as UserInfoExtension;
            return user != null;
        }
    }
}
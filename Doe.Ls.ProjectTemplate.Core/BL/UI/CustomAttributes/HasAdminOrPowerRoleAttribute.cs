using System.Web;
using System.Web.Mvc;
using Doe.Ls.EntityBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes
    {
    public class HasAdminOrPowerRoleAttribute : PositionEstablishmentRoleAttribute
        {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
            if(httpContext.SkipAuthorization) return true;
            if(!base.AuthorizeCoreBase(httpContext)) return false;
            var user = httpContext.Session[Cnt.CurrentUserKey] as UserInfoExtension;
            if(user != null && user.HasAdminOrPowerRole())
                {
                httpContext.SkipAuthorization = true;
                return true;
                }
            return false;
            }
        }
    public class MustHaveAdminOrPowerRoleAttribute : PositionEstablishmentRoleAttribute {
        protected override bool AuthorizeCore(HttpContextBase httpContext) {
            var user = httpContext.Session[Cnt.CurrentUserKey] as UserInfoExtension;
            if(user != null && user.HasAdminOrPowerRole()) {                
                return true;
                }
            return false;
            }
        }
    }
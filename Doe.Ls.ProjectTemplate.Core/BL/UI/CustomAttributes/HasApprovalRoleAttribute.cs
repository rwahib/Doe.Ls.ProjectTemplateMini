using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Doe.Ls.EntityBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes
    {
    public class HasApprovalRoleAttribute : PositionEstablishmentRoleAttribute
        {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
            if(httpContext.SkipAuthorization) return true;
            if(!base.AuthorizeCoreBase(httpContext)) return false;
            var user = httpContext.Session[Cnt.CurrentUserKey] as UserInfoExtension;
            if(user != null && user.HasApprovalRole())
                {
                httpContext.SkipAuthorization = true;
                return true;
                }
            return false; ;
            }
        }

    }

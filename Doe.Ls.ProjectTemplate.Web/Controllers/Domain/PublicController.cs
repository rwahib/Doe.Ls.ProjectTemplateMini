using System;
using System.Web.Mvc;
using System.Web.Security;
using Doe.Ls.EntityBase;
using Doe.Ls.ProjectTemplate.Core.DoeClients;

namespace Doe.Ls.ProjectTemplate.Web.Controllers.Domain
{
	public class PublicController : AppControllerBase
    {
        public ActionResult Confirmation()
        {
            var userName = String.Empty;

            if (Request.Form["userid"] != null)
            {
                var formUserName = Request.Form["userid"].Trim();
                var portalService = new PortalProxy();
                userName = portalService.decrypt(formUserName);
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidOperationException("could not authenticate the user");
            }

            FormsAuthentication.SetAuthCookie(userName, false);
            var uExt = LoginService.GetUserAndCacheItInDb(userName);
            
            
            var returnUrl = Request.Form[Cnt.SessionReturnUrl];

            if (!string.IsNullOrEmpty(returnUrl))
            {
                SessionService.AddToSession(Cnt.SessionReturnUrl, returnUrl);
                return Redirect(SessionService.ReadFromSession(Cnt.SessionReturnUrl).ToString());
            }

            ActionResult actionResult;
            if (GetDefaultAction(uExt, out actionResult)) return actionResult;

            return RedirectToAction("Index", "Home");
        }
    }
}

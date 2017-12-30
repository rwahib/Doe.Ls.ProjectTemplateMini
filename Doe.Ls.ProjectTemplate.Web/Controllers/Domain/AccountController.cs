using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.Settings;
using Cnt = Doe.Ls.EntityBase.Cnt;

namespace Doe.Ls.ProjectTemplate.Web.Controllers.Domain
    {
    public class AccountController : AppControllerBase
        {

        public ActionResult DoeSignIn(string returnUrl)
            {
            if(Request.IsAjaxRequest())
                {
                var msg = MessageHelper.AccessDeniedMessage();
                throw new InvalidOperationException(msg);
                }


            if(Request.QueryString.AllKeys.Contains(Cnt.SessionReturnUrl))
                {
                SessionService.AddToSession(Cnt.SessionReturnUrl, Request.QueryString[Cnt.SessionReturnUrl]);
                }

            if(!ProjectTemplateSettings.Portal.IsRealPortal)
                {
                if(ProjectTemplateSettings.Site.IsTestSite && ProjectTemplateSettings.Portal.DebugAutoAuthentication)
                    {
                    LoginService.GetUserAndCacheIt(ProjectTemplateSettings.Portal.DebugAutoAuthenticationUser);

                    FormsAuthentication.SetAuthCookie(ProjectTemplateSettings.Portal.DebugAutoAuthenticationUser, true);

                    var returnUrlLocation = SessionService.ReadFromSession(Cnt.SessionReturnUrl) ?? returnUrl;
                    if(returnUrlLocation != null)
                        {
                        return Redirect(returnUrlLocation.ToString());
                        }
                    else
                        {
                        return RedirectToAction("Index", "Home");
                        }


                    }



                var model = new UserInfoExtension { UserName = "" };

                SetSampleUsersToViewBag();
                return View(model);
                }

            return RedirectToAction("PortalSignIn", new { returnUrl = SessionService.ReadFromSession(Cnt.SessionReturnUrl) });
            }

        public ActionResult SignIn(string returnUrl)
            {
            return RedirectToAction("DoeSignIn", new { returnUrl });
            }

        private void SetSampleUsersToViewBag()
            {
            this.ServiceRepository.SysUserRepository().SetSampleUser(this.ViewData);

            }


        [HttpPost]
        public ActionResult DoeSignIn(UserInfoExtension model, string returnUrl)
            {
            model.UserName = model.UserName.ToLower();
            if(string.IsNullOrEmpty(returnUrl))
                {
                returnUrl = SessionService.ReadFromSession(Cnt.SessionReturnUrl)?.ToString();
                }


            if(ProjectTemplateSettings.Portal.IsRealPortal)
                {
                LoggerService.Log(new InvalidOperationException("Real portal users should not access the test page"));
                throw new InvalidOperationException("Real portal users should not access the test page");
                }

            var userInfo=LoginService.GetUserAndCacheItInDb(model.UserName);

            FormsAuthentication.SetAuthCookie(userInfo.UserName, true);

            LogUserActivity(Enums.LogActions.LogIn, userInfo.UserName, "Via form authentication (not Doe portal)");

            var returnUrlLocation = SessionService.ReadFromSession(Cnt.SessionReturnUrl) ?? returnUrl;
            if(returnUrlLocation != null)
                {
                return Redirect(returnUrlLocation.ToString());
                }

            return RedirectToAction("Dashboard", "User");
            }

        public void PortalSignIn(string returnUrl)
            {
            var loginUrl = string.Format(ProjectTemplateSettings.Portal.PageLogon,
                ProjectTemplateSettings.Portal.ConfirmationPageUrl);

            LogUserActivity(Enums.LogActions.LogIn, Enums.LogActions.LogIn.ToString(), "DoE user via portal");

            if(!string.IsNullOrEmpty(returnUrl))
                {
                loginUrl += "&" + Cnt.SessionReturnUrl + "=" + returnUrl;
                }

            Response.Redirect(loginUrl, false);
            }


        [Authorize]
        public ActionResult SignOff()
            {
            LogUserActivity(Enums.LogActions.LogOut, Enums.LogActions.LogOut.ToString());
            SessionService.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "Home");
            }
        }
    }

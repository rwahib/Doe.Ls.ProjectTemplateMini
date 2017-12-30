using System;
using System.Web.Mvc;
using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;


namespace Doe.Ls.ProjectTemplate.Web.Controllers
{
    public class AppApiController : AppControllerBase
    {

        public AppApiController()
        {

        }
        public AppApiController(ServiceRepository repository)
        {
            this._serviceRepository = repository;
        }
        private IUserIdentityService UserIdentityService
        {
            get { return ServiceRepository.UserIdentityService(); }
        }

        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult GetUserInfoFromADByEmail(string email)
        {
            UserInfo userInfo = null;

            try
            {
                userInfo = UserIdentityService.GetUserByEmail(email);
            }
            catch (Exception exception)
            {
                LoggerService.Log(new Exception($"{email} is not a valid DoE email", exception));
            }
            // TODO: Retrieve User Information from DB and populate UserInfoExtension
            // Example
            /***
             * var user = UserRepository.GetUserByUserName(userInfo.UserName);
             * if (user != null)
             * {
             *      var userinfoextn = UserInfoExtension.MapUserInfo(userInfo);
             *      userinfoextn.BaseSchoolCode = user.BaseSchoolCode;
             *      
             *      return Json(userinfoextn, JsonRequestBehavior.AllowGet);
             * }
             ***/

            return Json(userInfo, JsonRequestBehavior.AllowGet);
        }

        public bool SetUtilityStatus(bool collapsed)
        {
            SessionService.AddToSession("utility-collapse", collapsed);
            return collapsed;

        }

        public bool GetUtilityStatus()
        {
            if (SessionService.ReadFromSession("utility-collapse") == null) return false;
            var collapse = false;
            if (bool.TryParse(SessionService.ReadFromSession("utility-collapse").ToString(), out collapse))
                return collapse;
            else return false;

        }


        public ActionResult GetCurrentUser()
        {
            UserInfoExtension result = null;

            if (CurrentUser != null && CurrentUser.IsRoleInitialised && !string.IsNullOrEmpty(CurrentUser.UserName))
            {
                result = CurrentUser;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

      

     

       }
}



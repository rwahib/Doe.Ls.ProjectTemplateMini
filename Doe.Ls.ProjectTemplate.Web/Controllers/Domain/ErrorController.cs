#region

using System;
using System.Web.Mvc;
using Doe.Ls.ProjectTemplate.Core.BL.UI;

#endregion

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{
    public class ErrorController : AppControllerBase
    {
        /// <summary>
        ///   for normal calls
        /// </summary>
        /// <returns> </returns>
        public ActionResult Index()
        {
            if (ViewData.Model == null)
            {
                ViewData.Model = HttpContext.Error ?? new Exception("Error");
            }

            Server.ClearError();
            ViewBagWrapper.InfoBag.SetTitle("",ViewData);

            if (Request != null && Request.IsAjaxRequest())
            {
                return PartialView("Error/Detail", ViewData.Model);
            }
            //
            return View("Error/Index", ViewData.Model);
        }

        /// <summary>
        ///   for ajax calls
        /// </summary>
        /// <returns> </returns>
        public ActionResult IndexAjax()
        {
            return PartialView("Error/Detail", ViewData.Model);
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 200;
            return View("Error/NotFound");

        }
    }
}
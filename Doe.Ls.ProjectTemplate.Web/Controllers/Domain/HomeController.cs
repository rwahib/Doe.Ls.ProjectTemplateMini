using System;
using System.Web.Mvc;
using Doe.Ls.ProjectTemplate.Core.BL.UI;

namespace Doe.Ls.ProjectTemplate.Web.Controllers.Domain
    {
        public class HomeController : AppControllerBase
        {
        public ActionResult Index()
        {
            //ViewBagWrapper.InfoBag.SetDescription("Modify this template to jump-start your ASP.NET MVC application.", ViewData);

            //if (CurrentUser != null)
            //    Console.WriteLine("user still exists");

            //return View();

            
            if (CurrentUser != null)
            {
                return RedirectToAction("Dashboard", "User");
            }
            else
            {
                return RedirectToAction("DoeSignIn", "Account");
            }
        }

        public ActionResult About()
        {

            ViewBagWrapper.InfoBag.SetDescription("Your app description page.", ViewData);
            return View();
        }

        public ActionResult Contact()
        {
            ViewBagWrapper.InfoBag.SetDescription("Your contact list here.", ViewData);

            return View();
        }
        
        }
    }

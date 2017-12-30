using System.Web.Mvc;
using Doe.Ls.ProjectTemplate.Core.BL.UI;

namespace Doe.Ls.ProjectTemplate.Web.Controllers.Domain
{
    public class TestController : AppControllerBase
    {

        public ActionResult Help()
        {
            ViewBagWrapper.InfoBag.SetDescription("Help", ViewData);

            return View();
        }

        public ActionResult Index()
        {
            ViewBagWrapper.InfoBag.SetDescription("This Action for some tests.", ViewData);

            return View();
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

        public ActionResult Dialog()
        {
            ViewBagWrapper.InfoBag.SetTitle("Debug Code", ViewData);
            ViewBagWrapper.InfoBag.SetDescription("Your debuging code goes here", ViewData);

            return View("Debug");
        }

        public ActionResult Require()
        {
            ViewBagWrapper.InfoBag.SetTitle("Debug Code", ViewData);
            ViewBagWrapper.InfoBag.SetDescription("Require test", ViewData);

            return View("RequireTest");
        }

        public ActionResult BootStrapToGef()
        {
            return View();
        }

        public ActionResult GefStyles()
        {
            return View();
        }

        public ActionResult Lists()
        {
            return View();
            }
    }
}

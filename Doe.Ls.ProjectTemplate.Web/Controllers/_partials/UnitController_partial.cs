using System.Web.Mvc;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {


    [System.Web.Mvc.Authorize(Roles = Enums.UserRoleValues.DoEUser)]
    public partial class UnitController
        {
        /// <summary>
        /// for unit test injection
        /// </summary>
        /// <param name="service"></param>
        public UnitController(ServiceRepository service)
            {
            _serviceRepository = service;
            _repository = service.UnitRepository();
            }
        public UnitController()
            {            
            }
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult GetUnits(int bUnitId = 0, bool displayNumbers = true, bool fromChart=false)
        {
            if (displayNumbers)
            {
                
                var units = Repository.GetUnitsWithPositionCount(bUnitId);
                return Json(units, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var units = Repository.GetUnits(bUnitId);
                return Json(units, JsonRequestBehavior.AllowGet);
            }
        }

        
        }
    }
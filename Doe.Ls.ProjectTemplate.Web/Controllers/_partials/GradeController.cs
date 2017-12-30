using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{
    [System.Web.Mvc.Authorize(Roles = Enums.UserRoleValues.DoEUser)]
    public partial class GradeController
    {
        /// <summary>
        /// for unit test injection
        /// </summary>
        /// <param name="service"></param>
        public GradeController(ServiceRepository service)
        {
            _serviceRepository = service;
            _repository = service.GradeRepository();
        }
        public GradeController()
        {
        }

    }
}
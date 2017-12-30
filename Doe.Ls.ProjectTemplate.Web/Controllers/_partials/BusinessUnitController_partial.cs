using Doe.Ls.ProjectTemplate.Core.BL;


namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {

    public partial class BusinessUnitController
    {
        public BusinessUnitController()
            {
            }
        /// <summary>
        /// for unit test injection
        /// </summary>
        /// <param name="service"></param>
        public BusinessUnitController(ServiceRepository service)
            {
            _serviceRepository = service;
            _repository = service.BusinessUnitRepository();
            }
        }
    }
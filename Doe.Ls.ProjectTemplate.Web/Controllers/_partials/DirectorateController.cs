using Doe.Ls.ProjectTemplate.Core.BL;


namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {

    public partial class DirectorateController
        {
        public DirectorateController()
            {
            }
        /// <summary>
        /// for unit test injection
        /// </summary>
        /// <param name="service"></param>
        public DirectorateController(ServiceRepository service)
            {
            _serviceRepository = service;
            _repository = service.DirectorateRepository();
            }
        }
    }
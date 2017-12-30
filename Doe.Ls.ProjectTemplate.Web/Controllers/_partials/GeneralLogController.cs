using Doe.Ls.ProjectTemplate.Core.BL;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{
    
    public partial class GeneralLogController
        {
        /// <summary>
        /// for unit test injection
        /// </summary>
        /// <param name="service"></param>
        public GeneralLogController(ServiceRepository service)
        {
            _serviceRepository = service;
            _repository = service.GeneralLogRepository();
        }
        public GeneralLogController()
        {
        }

    }
}
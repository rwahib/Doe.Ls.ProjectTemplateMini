using Doe.Ls.ProjectTemplate.Core.Settings;

namespace Doe.Ls.ProjectTemplate.Core.DoeClients
{
    public class PortalProxy:PartnerAppWs.PartnerAppService
    {
        public PortalProxy()
        {
            Url = ProjectTemplateSettings.Portal.DetPortalISecurityFactory;
        }
    }
}

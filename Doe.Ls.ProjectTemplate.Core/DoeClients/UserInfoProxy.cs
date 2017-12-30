using Doe.Ls.ProjectTemplate.Core.Settings;

namespace Doe.Ls.ProjectTemplate.Core.DoeClients
{
    public class UserInfoProxy: VleWsUserInformation.UserInformation
    {
        public UserInfoProxy()
        {
            Url = ProjectTemplateSettings.Portal.DecUserInfoServiceUrl;
        }
    }
}

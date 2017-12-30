using Doe.Ls.ProjectTemplate.Core.Settings;

namespace Doe.Ls.ProjectTemplate.Core.DoeClients
{
    public class LoggerServiceProxy : VleWsLoggerService.CilLoggerService
    {
        public LoggerServiceProxy()
        {
            Url = ProjectTemplateSettings.Logger.AppLoggerServiceUrl;
        }

        public int ApplicationId
        {
            get { return int.Parse(ProjectTemplateSettings.Logger.AppLoggerId); }
        }
    }
}
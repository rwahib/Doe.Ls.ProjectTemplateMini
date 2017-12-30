using System;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.DoeClients;
using Doe.Ls.ProjectTemplate.Core.Settings;
using Doe.Ls.ProjectTemplate.Core.VleWsLoggerService;

using LogCategory = Doe.Ls.ProjectTemplate.Core.VleWsLoggerService.LogCategory;
using LogType = Doe.Ls.ProjectTemplate.Core.VleWsLoggerService.LogType;
using Unity.Attributes;
namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices.Logging
{
    public class DecLsLoggerService : ILoggerService
    {
        private readonly LoggerServiceProxy _client;

        public DecLsLoggerService()
        {
            _client = new LoggerServiceProxy();
        }

        public void Log(Exception exception)
        {
            _client.RegisterDBLogItemAsync(GetLogItem(exception));            
        }

        public void SendMail(Exception exception)
        {
            var message = new MailMessage();
            var tos = ProjectTemplateSettings.Notification.AdminEmailAddresses.Split(',');
            foreach (var to in tos)
            {
                message.To.Add(to);
            }

            message.Subject = $"Error generated : {exception.Message}".CleanFromControlCharacters();
            message.From = new MailAddress(ProjectTemplateSettings.Notification.NotificationEmailAddress);
            message.Body = exception.ToString();
            message.IsBodyHtml = true;
            message.BodyEncoding = new UnicodeEncoding();
            var smtp = new SmtpClient(ProjectTemplateSettings.Notification.Host);

            AddCientInformation(ref message);
            new Task(() =>
            {
                smtp.Send(message);                 
                }).Start();
            }

        private string GetFullException(Exception exception)
        {
            return exception.ToString();
        }

        private LogItem GetLogItem(Exception exception)
        {

            var logItem = new LogItem
            {
                ApplicationID = _client.ApplicationId,
                UserName = LoggerHelper.GetCurrentUser(),
                Category = LogCategory.BusinessLayer,
                CreatedDate = DateTime.Now,
                FriendlyMessage = exception.Message,
                MachineName = Environment.MachineName,
                FullDescription = GetFullException(exception),
                LogType = LogType.Error,
                Severity = LogSeverity.High,
                Title = exception.Message

            };
            AddCientInformation(ref logItem);
            return logItem;
        }

        private void AddCientInformation(ref LogItem logItem)
        {
            logItem.FullDescription += $"\n\r <br/>{EntityBase.Logging.LoggerHelper.GetClientLogInfo()}";

        }
        private void AddCientInformation(ref MailMessage message)
        {
            message.Body += $"\n\r <br/>{EntityBase.Logging.LoggerHelper.GetClientLogInfo()}";
        }

        [Unity.Attributes.Dependency]
        public ISessionService SessionService { get; set; }
    }
}

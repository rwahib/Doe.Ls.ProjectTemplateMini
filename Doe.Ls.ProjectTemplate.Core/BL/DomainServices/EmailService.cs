using System;
using System.Net.Mail;
using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.Settings;

using Unity.Attributes;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices
{
    public class EmailService : IEmailService
    {
        [Unity.Attributes.Dependency]
        public ILoggerService LoggerService { get; set; }
        [Unity.Attributes.Dependency]
        public IRepositoryFactory RepositoryFactory { get; set; }
        [Unity.Attributes.Dependency]
        public ISessionService SessionService { get; set; }

        public Result SendEmail(EmailMessage msg)
        {
            var message = new MailMessage();
            message.To.Add(ProjectTemplateSettings.Notification.UseRealEmail
                ? msg.To
                : ProjectTemplateSettings.Notification.AdminEmailAddresses);
            // message.To.Add(msg.To);
            if (!string.IsNullOrWhiteSpace(msg.Cc))
            {
                message.To.Add(msg.Cc);
            }
            message.Subject = msg.Subject;
            message.From = new MailAddress(msg.From);
            message.Body = msg.Message;
            if (!string.IsNullOrWhiteSpace(msg.Bcc))
            {
                message.Bcc.Add(msg.Bcc);
            }
            message.IsBodyHtml = true;
            if (msg.Attachment != null)
            {
                message.Attachments.Add(msg.Attachment);
            }

            var smtp = new SmtpClient(ProjectTemplateSettings.Notification.Host);
            try
            {
                smtp.Send(message);
                return new Result
                {
                    Status = Status.Success,
                    Message = Status.Success.ToString()
                };
            }
            catch (Exception ex)
            {
                var exMsg = ex.Message + LoggerHelper.GetClientLogInfo();
                return new Result
                {
                    Status = Status.Error,
                    Message = exMsg
                };
            }
        }

        public void Dispose()
        {
            Dispose();
        }
    }
}

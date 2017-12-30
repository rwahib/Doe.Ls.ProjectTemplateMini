using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Web;
using Doe.Ls.EntityBase.Models;

namespace Doe.Ls.EntityBase.Logging
{
    public class LoggerHelper
    {
        public static string GetClientLogInfo()
        {
            var httpUser = string.Empty;
            var user = string.Empty;
            var sb = new StringBuilder();
            if (HttpContext.Current != null)
            {
                httpUser = HttpContext.Current.User.Identity.Name;
            }

            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            user = windowsIdentity.Name;

            sb.AppendLine($"<br/>Web user={httpUser}");
            sb.AppendLine($"<br/>Server user={user}");
            sb.AppendLine($"<br/>Machine={Environment.MachineName}");
            sb.AppendLine($"<br/>UserDomainName={Environment.UserDomainName}");
            sb.AppendLine($"<br/>UserName={Environment.UserName}");

            
            if (HttpContext.Current != null)
            {
                sb.AppendLine($"<br/>Request URL ={HttpContext.Current.Request.RawUrl}");
                if(HttpContext.Current.Request.UrlReferrer != null) sb.AppendLine($"<br/>Referrer URL ={HttpContext.Current.Request.UrlReferrer.AbsoluteUri}");
                sb.AppendLine($"<br/>Browser ={HttpContext.Current.Request.Browser.Browser}");
                sb.AppendLine($"<br/>Version ={HttpContext.Current.Request.Browser.Version}");
                sb.AppendLine($"<br/>UserAgent ={HttpContext.Current.Request.UserAgent}");
                }
                
            

            return sb.ToString();
        }

        public static string GetCurrentUser()
        {
            string httpUser;
            string user;

            var sb = new StringBuilder();
            if (HttpContext.Current != null)
            {
                httpUser = HttpContext.Current.User.Identity.Name;
                return httpUser;
            }

            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (windowsIdentity != null)
            {
                user = windowsIdentity.Name;
                return user;
            }

            return Cnt.Anonymous;
        }

        public static LogModel ExceptionToLogModel(Exception exception, int applicationId)
        {
            return new LogModel
            {
                LogCategory = LogCategory.BusinessLayer,
                Severity = Severity.High,
                MessageBody = exception.ToString(),
                LogGuid = Guid.NewGuid(),
                ApplicationId = applicationId,
                CreatedDate = DateTime.Now,
                FriendlyMessage = exception.Message,
                FullTraceOrDescription = exception.StackTrace,
                LogType = LogType.Error,
                Status = Status.Error,
                UserName = GetCurrentUser(),
                MachineName = Environment.MachineName,
                Source = Environment.MachineName
            };

        }

        public static string GetFullHtmContent(LogModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<br>");

            sb.Append(model.FriendlyMessage);
            sb.Append("<br>");

            sb.Append(model.MessageBody);
            sb.Append("<br>");

            sb.Append(model.FullTraceOrDescription);
            sb.Append("<br>");

            sb.Append(model.Source);
            sb.Append("<br>");

            sb.Append(model.MachineName);
            sb.Append("<br>");

            sb.Append(model.UserName);
            sb.Append("<br>");

            return sb.ToString();
        }

        public static IMessageFormatter GetLogXmlFormatter()
        {
            return new XmlMessageFormatter(new Type[] { typeof(LogModel), typeof(EmailHeaderModel), });
        }

        public static IMessageFormatter GetJsonFormatter()
        {
            return new JsonLogFormatter();
        }
    }

    public class JsonLogFormatter : IMessageFormatter
    {
        public object Clone()
        {
            return new JsonLogFormatter();
        }

        public bool CanRead(Message message)
        {
            return true;
        }

        public object Read(Message message)
        {
            var reader = new StreamReader(message.BodyStream);
            var st = reader.ReadToEnd();
            return st.JsonDeserialise<LogModel>();
        }

        public void Write(Message message, object obj)
        {
            if (message.Body is LogModel)
            {

                var serialised = (message.Body as LogModel).JsonSerialise();
                var byteArray = Encoding.UTF8.GetBytes(serialised);
                message.BodyStream = new MemoryStream(byteArray);
                message.Body = null;
            }
            else
            {
                throw new NotSupportedException("object is not LogModel");
            }
        }
    }
}

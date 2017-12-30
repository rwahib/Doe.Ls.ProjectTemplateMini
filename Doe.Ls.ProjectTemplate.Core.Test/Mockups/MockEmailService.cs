using System.Collections.Generic;
using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;


namespace Doe.Ls.ProjectTemplate.Core.Test.Mockups
{
    public class MockEmailService : IEmailService
    {
        public List<string> MessageList { get; set; }


        public MockEmailService()
        {
            MessageList = new List<string>();
        }

        public MockEmailService(List<string> messageList)
        {
            if (messageList == null) messageList = new List<string>();
            MessageList = messageList;
        }

        public void Dispose()
        {
            MessageList.Add("Dispose called");
        }

        [Unity.Attributes.Dependency]
        public ILoggerService LoggerService { get; set; }

        [Unity.Attributes.Dependency]
        public IRepositoryFactory ServiceFactory { get; set; }

        [Unity.Attributes.Dependency]
        public IRepositoryFactory RepositoryFactory { get; set; }

        [Unity.Attributes.Dependency]
        public ISessionService SessionService { get; set; }

        public Result SendEmail(EmailMessage msg)
        {
            MessageList.Add("SendEmail called:" + msg.Subject);

            return new Result();
        }
    }
}
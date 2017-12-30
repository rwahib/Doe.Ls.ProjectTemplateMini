using System;
using System.Collections.Generic;
using Doe.Ls.EntityBase.Logging;

namespace Doe.Ls.ProjectTemplate.Core.Test.Mockups
{
    public class MockoggerService : ILoggerService
    {
        public List<string> MessageList { get; set; }

        public MockoggerService()
        {
            MessageList = new List<string>();

        }
        public void Log(Exception ex)
        {
            MessageList.Add("Log calledL:"+ex.ToString());
        }

        public void SendMail(Exception exception)
        {
            MessageList.Add("SendMail calledL:" + exception.ToString());
        }
    }
}
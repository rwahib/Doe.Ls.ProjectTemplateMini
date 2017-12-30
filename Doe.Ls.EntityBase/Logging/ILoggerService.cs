using System;

namespace Doe.Ls.EntityBase.Logging
{
    public interface ILoggerService
    {
        void Log(Exception ex);
        void SendMail(Exception ex);
    }
}

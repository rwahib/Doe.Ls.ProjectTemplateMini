using System;
using Doe.Ls.EntityBase.Models;

namespace Doe.Ls.EntityBase.Logging
{
    public class LogModel
    {
        public int? LogId { get; set; }
        public Guid? LogGuid { get; set; }
        public int ApplicationId { get; set; }
        public LogType LogType { get; set; }
        public LogCategory LogCategory { get; set; }
        public string MachineName { get; set; }
        public string Source { get; set; }
        public string MessageBody { get; set; }
        public string FriendlyMessage { get; set; }
        public string FullTraceOrDescription { get; set; }
        public Status Status { get; set; }
        public string UserName { get; set; }
        public string TagOrCategory { get; set; }
        public Severity Severity { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool NotifyByEmail { get; set; }
        public EmailHeaderModel EmailHeader { get; set; }
        public override string ToString()
        {
            return $"{LogType}-{FriendlyMessage}-{CreatedDate}";
        }
    }
}

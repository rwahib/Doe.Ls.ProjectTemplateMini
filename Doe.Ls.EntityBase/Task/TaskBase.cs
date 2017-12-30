using System;
using System.Configuration;
using System.Text;

namespace Doe.Ls.EntityBase.Task {

    public abstract class TaskBase : ITask {
        private readonly string _taskName;
        private readonly string _description;
        private readonly bool _active;

        #region ITask Members

        public event EventHandler Starting;

        public event EventHandler Completed;


        public string TaskName
        {
            get => _taskName;
            set { throw new AccessViolationException("This property is readonly"); }
        }

        public string Description {
            get => _description;
            set => throw new AccessViolationException("This property is readonly");
        }

        public bool TraceEnabled { get; set; }
        public ITraceListener TraceListener { get; set; }
        public ConfigurationElement ConfigurationElement { get; set; }

        public bool Active
        {
            get => _active;
            set => throw new AccessViolationException("This property is readonly");
        }

        protected TaskBase()
            : base()
        {

            var name = this.GetType().Name.Wordify();
            this._taskName = name;
            this._description = name;
            this._active = true;
        }

        protected TaskBase(string name, string description, bool active, bool enableTrace)
            : base() {
            this._taskName = name;
            this._description = description;
            this._active = active;
            this.TraceEnabled = enableTrace;
        }

        public void Run([System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
       [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
       [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            TraceListener.Trace("\n");
           TraceListener.Trace($"******** Starting {TaskName} Task **********");
            
            string traceArg = GetTraceArg(memberName, sourceFilePath, sourceLineNumber);
            TraceListener.Trace(traceArg);
            if (Starting != null) Starting(this, new MessageEventArgs($"Starting at {DateTime.Now}"));
            ExecuteTask();
            if (Completed != null) Completed(this, new MessageEventArgs($"Completed at {DateTime.Now}"));

            TraceListener.Trace($"******** End {TaskName} Task **********");
            TraceListener.Trace("\n");
        }

        private string GetTraceArg(string memberName, string sourceFilePath, int sourceLineNumber) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"memberName: {memberName} ");
            sb.AppendLine($"sourceFilePath: {sourceFilePath} ");
            sb.AppendLine($"sourceLineNumber: {sourceLineNumber} ");

            return sb.ToString();
        }

        #endregion

        public abstract void ExecuteTask();

       
    }
}

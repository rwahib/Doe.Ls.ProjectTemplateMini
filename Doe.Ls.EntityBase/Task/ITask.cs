using System;
using System.Configuration;

namespace Doe.Ls.EntityBase.Task {
    public interface ITask {
        event EventHandler Starting;

        event EventHandler Completed;
    
        string TaskName {
            get;
            set;
        }

        string Description {
            get;
            set;
        }

        bool Active {
            get;
            set;
        }

        bool TraceEnabled {
            get;
            set;
        }
        ITraceListener TraceListener { get; set; }

        ConfigurationElement ConfigurationElement { get; set; }
        void Run([System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0);
    }
}

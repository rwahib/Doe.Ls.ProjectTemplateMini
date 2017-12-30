using System.Collections;

namespace Doe.Ls.EntityBase.Task
{
    public interface ITraceListener
    {

        void StartSessionTrace(string message);
        void EndSessionTrace(string message);
        void Trace(string message,bool newLine=true);
        void Trace(IEnumerable objects, bool newLine = true);
    }
}
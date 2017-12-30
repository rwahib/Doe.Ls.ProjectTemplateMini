using System;

namespace Doe.Ls.EntityBase.Task
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public MessageEventArgs(string message):base()
        {
            this.Message = message;
        }
    }
}
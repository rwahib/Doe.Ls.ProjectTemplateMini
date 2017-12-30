using System.Collections.Generic;
using System.Messaging;

namespace Doe.Ls.EntityBase.MsgQueue {
    public interface IMsgQueueService {
        MessageQueue MessageQueue { get; }

        void SendMessage(string body, string label);
        void SendMessage(Message msg);
        bool UnitTest { get; set; }
        Message ReceiveMessage(bool waiting = false);

        void CreateMessageQueue();
        /// <summary>
        /// with consumption 
        /// </summary>        
        List<Message> ReceiveMessageList();

        /// <summary>
        /// no consumption 
        /// </summary>        
        Message[] GetCopyOfMessages(int top);

         
        Message PeekMessage(bool waiting = false);
        bool HasMessage();
        bool Exists();
        void PurgeQueue();
        void Delete();
    }
}
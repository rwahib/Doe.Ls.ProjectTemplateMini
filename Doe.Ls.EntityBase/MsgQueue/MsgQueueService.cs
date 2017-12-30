using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using System.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Doe.Ls.EntityBase.MsgQueue {
    public class MsgQueueService : IMsgQueueService {

        private readonly TimeSpan _defaultTimeOut = new TimeSpan(0, 0, 0, 0, 1);

        private readonly MsgQueueServiceSettings _settings;
        private readonly IMessageFormatter _formatter;

        private MessageQueue _messageQueue;

        public MessageQueue MessageQueue {
            get { return _messageQueue; }
        }

        public MsgQueueService(MsgQueueServiceSettings settings, IMessageFormatter formatter = null) {
            _settings = settings;
            _formatter = formatter;
            _messageQueue = new MessageQueue(_settings.MessageQueuePath, QueueAccessMode.SendAndReceive);

        }
        /// <summary>
        /// Create internal Queue
        /// </summary>
        public void CreateMessageQueue() {
            _messageQueue = MessageQueue.Create(_settings.MessageQueuePath, _settings.Transactional);
            //_messageQueue.QueueName = _settings.QueueFriendlyName;
            _messageQueue.DefaultPropertiesToSend.Recoverable = _settings.Recoverable;

        }

        public void SendMessage(string body, string label) {

            var queuePath = _settings.MessageQueuePath;

            try {

                var msg = new Message {
                    UseAuthentication = false,
                    Recoverable = true,
                    Label = label,
                    Body = body
                };

                _messageQueue.Send(msg, MessageQueueTransactionType.Automatic);


            } catch (Exception ex) {
                throw ex;
            } finally {
                if (_messageQueue != null) _messageQueue.Close();
            }

        }

        public void SendMessage(Message msg)
        {
            Check(_formatter);
            try {
                if (!(msg.Body is string))
                {
                    var serialised = new object();
                    _formatter.Write(msg, serialised);
                }

                _messageQueue.Send(msg, MessageQueueTransactionType.Automatic);

            } catch (Exception ex) {
                throw ex;
            } finally {
                if (_messageQueue != null) _messageQueue.Close();
            }

        }

        private void Check(IMessageFormatter formatter)
        {
            if (formatter == null)
            {
                
                throw new NullReferenceException("formatter must be provided");
            }
        }

        public bool UnitTest { get; set; }

        public Message ReceiveMessage(bool waiting = false) {

            MessageQueue msmq = null;
            Message msg = null;
            var queuePath = _settings.MessageQueuePath;
            try {
                msmq = new MessageQueue(queuePath, QueueAccessMode.SendAndReceive);
                if (waiting) {
                    msg = msmq.Receive(MessageQueueTransactionType.Automatic);
                } else {
                    if (HasMessage()) {
                        msg = msmq.Receive(MessageQueueTransactionType.Automatic);
                    }
                }

                SetFormatter(msg);

            } finally {
                if (msmq != null) msmq.Close();
            }

            SetFormatter(msg);
            return msg;
        }

        /// <summary>
        /// with consumption 
        /// </summary>        
        /// 
        public List<Message> ReceiveMessageList() {
            var list = new List<Message>();
            while (HasMessage()) {
                list.Add(ReceiveMessage());
            }

            return list;
        }

        /// <summary>
        /// no consumption 
        /// </summary>        
        public Message[] GetCopyOfMessages(int top) {

            return _messageQueue.GetAllMessages().Take(top).Select(m => {
                m.Formatter = this._formatter;
                return m;
            }).ToArray();

        }

        public Message PeekMessage(bool waiting = false) {
            MessageQueue msmq = null;

            var queuePath = _settings.MessageQueuePath;

            msmq = new MessageQueue(queuePath, QueueAccessMode.SendAndReceive);
            Message msg = null;
            try {
                if (waiting) {
                    msg = msmq.Peek();
                } else {
                    if (HasMessage()) {
                        msg = msmq.Peek();
                    }
                }
            } finally {
                msmq.Close();
            }
            SetFormatter(msg);
            return msg;
        }

        public bool HasMessage() {
            MessageQueue msmq = null;
            var queuePath = _settings.MessageQueuePath;
            msmq = new MessageQueue(queuePath, QueueAccessMode.SendAndReceive);
            Message msg = null;
            try {
                msg = msmq.Peek(_defaultTimeOut);
            } catch{
                
                return false;
            }
            msmq.Close();
            return msg != null;
        }

        public bool Exists() {
            if (_settings.Remote) {
                throw new InvalidOperationException("Exists should be called with local only");
            }
            var queuePath = _settings.MessageQueuePath;
            return MessageQueue.Exists(queuePath);
        }

        public void PurgeQueue() {
            MessageQueue msmq = null;
            var queuePath = _settings.MessageQueuePath;

            msmq = new MessageQueue(queuePath, QueueAccessMode.SendAndReceive);

            msmq.Purge();
            msmq.Close();

        }

        public void Delete() {
            if (_settings.Remote) {
                throw new InvalidOperationException("Delete should be called with local only");
            }
            if (Exists()) {
                MessageQueue.Delete(_settings.MessageQueuePath);
            } else {
                throw new ObjectNotFoundException(_settings.MessageQueuePath);
            }

        }

        public bool CanMessageQueueSendAndReceive() {

            var msg = new Message("test");
            this.SendMessage(msg);
            var id = msg.Id;

            msg = this.MessageQueue.ReceiveById(id);

            if (msg.Id == id) return true;

            throw new ObjectNotFoundException("Message is not found");
        }

        public void ReceiveListener(Func<Message, ReceiveActionArgument, bool> messageListener, bool newThread = false) {
            if (!newThread) {
                ReceiveListenerCore(messageListener);
            } else {
                new Thread(() => {
                    ReceiveListenerCore(messageListener);
                }).Start();
            }
        }

        public void PeekListener(Func<Message, PeekActionArgument, bool> messageListener, bool newThread = false) {
            if (!newThread) {
                PeekListenerCore(messageListener);
            } else {
                new Thread(() => {
                    PeekListenerCore(messageListener);
                }).Start();
            }

        }

        private void PeekListenerCore(Func<Message, PeekActionArgument, bool> messageListener) {
            var arg = new PeekActionArgument { PeekAction = PeekAction.Continue };
            while (arg.PeekAction == PeekAction.Continue) {
                var message = this.PeekMessage(true);
                if (messageListener != null) {
                    messageListener(message, arg);
                    if (arg.PeekAction == PeekAction.Stop) return;
                }
            }
        }

        private void ReceiveListenerCore(Func<Message, ReceiveActionArgument, bool> messageListener) {
            var arg = new ReceiveActionArgument { ReceiveAction = ReceiveAction.Continue };
            while (arg.ReceiveAction == ReceiveAction.Continue) {
                var message = this.ReceiveMessage(true);
                if (messageListener != null) {
                    messageListener(message, arg);
                    if (arg.ReceiveAction == ReceiveAction.Stop) return;
                }
            }
        }

        private void SetFormatter(Message msg) {
            if (msg != null) {
                if (_formatter != null) msg.Formatter = _formatter;

            }
            else
            {
                throw new InvalidOperationException();
            }
        }


    }
}
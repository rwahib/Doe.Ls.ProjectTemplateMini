using System;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Web.Helpers;
using Doe.Ls.EntityBase.MVCExtensions;

namespace Doe.Ls.EntityBase.MsgQueue
{
    public class MsgQueueServiceSettings
    {
        public string MessageQueuePath { get; set; }
        public string QueueFriendlyName { get; set; }
        public bool Transactional { get; set; }
        public bool Recoverable { get; set; }
        public string PrefixLabel { get; set; }
        public bool Remote { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public override string ToString()
        {
            return MessageQueuePath;
        }




        public string GetMessageRootPath()
        {

            var index = MessageQueuePath.IndexOf('$');
            if (index < 0)
            {
                throw new InvalidOperationException($" Path {MessageQueuePath} is not private ");
            }
            var root = MessageQueuePath.Substring(0, index + 2);
            return root.Trim();
        }

        public string GetQueueFullPath(string qName)
        {
            return string.Concat(GetMessageRootPath(), qName);
        }

        public static MsgQueueServiceSettings GetFromConfig(string connectionName)
        {
            if (ConfigurationManager.ConnectionStrings[connectionName] != null)
            {
                var connection = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

                var settings = connection.JsonDeserialise<MsgQueueServiceSettings>();
                return settings;
            }

            throw new ObjectNotFoundException($"Connection name {connectionName} is not found");
        }

        public const string UNIT_TEST_CONNECTION_NAME = "vle-unit-test-message-queue";
        public const string UNIT_TEST_LOGGER_CONNECTION_NAME = "vle-logger-unit-test-message-queue";
        public const string LOGGER_CONNECTION_NAME = "vle-logger-message-queue";
    }
}
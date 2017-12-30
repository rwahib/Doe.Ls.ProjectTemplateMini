using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI
{
    public class MessageHelper
    {
        public static string NotFoundMessage(string itemName,IRepositoryFactory factory=null)
        {
            factory = factory ?? new RepositoryFactory();
            factory.RegisterAllDependencies();
            var msgSrv = MessageFactory.GetMessageService(factory);
            
            var messageObj = msgSrv.GetMessageByCode("ERR-NOT-FOUND", true);
            if (string.IsNullOrEmpty(itemName))
            {
                itemName = "object";
            }
            if (messageObj != null)
            {
                var msg = string.Format(messageObj.MessageFormat, itemName);
                return msg;
            }
            
            return $"The {itemName} was not found";
          
        }

        public static string NullPleaseEnterMessage(string itemName,IRepositoryFactory factory=null)
        {
            factory = factory ?? new RepositoryFactory();
            factory.RegisterAllDependencies();
            var msgSrv = MessageFactory.GetMessageService(factory);
            if (string.IsNullOrEmpty(itemName))
            {
                itemName = "object";
            }
            var messageObj = msgSrv.GetMessageByCode("ERR-NULL-PLEASE-ENTER", true);
            if (messageObj != null)
            {
                var msg = string.Format(messageObj.MessageFormat, itemName);
                return msg;
            }

            return $"Please enter a {itemName}.";

        }

        public static string NullPleaseSelectMessage(string itemName,IRepositoryFactory factory=null)
        {
            factory = factory ?? new RepositoryFactory();
            factory.RegisterAllDependencies();
            var msgSrv = MessageFactory.GetMessageService(factory);
            if (string.IsNullOrEmpty(itemName))
            {
                itemName = "object";
            }
            var messageObj = msgSrv.GetMessageByCode("ERR-NULL-PLEASE-SELECT", true);
            if (messageObj != null)
            {
                var msg = string.Format(messageObj.MessageFormat, itemName);
                return msg;
            }

            return $"Please select a {itemName}.";

        }
        public static string DocNumExistsMessage(IRepositoryFactory factory = null)
        {
            factory = factory ?? new RepositoryFactory();
            factory.RegisterAllDependencies();
            var msgSrv = MessageFactory.GetMessageService(factory);
           
            var messageObj = msgSrv.GetMessageByCode("ERR-DOCNUM-EXISTS", true);
            if (messageObj != null)
            {
                var msg = string.Format(messageObj.MessageFormat);
                return msg;
            }

            return "This document number already exists.";

        }

        public static string RdPdMustExistsBeforePositionMessage(IRepositoryFactory factory = null)
        {
            factory = factory ?? new RepositoryFactory();
            factory.RegisterAllDependencies();
            var msgSrv = MessageFactory.GetMessageService(factory);

            var messageObj = msgSrv.GetMessageByCode("ERR-RD-PD-MUST-EXISTS-BEFORE-POSITION", true);
            if (messageObj != null)
            {
                var msg = string.Format(messageObj.MessageFormat);
                return msg;
            }

            return "Error. A new position needs to be created before a role/position description can be created.";
        }


        public static string AccessDeniedMessage( IRepositoryFactory factory = null)
        {
            factory = factory ?? new RepositoryFactory();
            factory.RegisterAllDependencies();
            var msgSrv = MessageFactory.GetMessageService(factory);

            var messageObj = msgSrv.GetMessageByCode("ERR-ACCESS-DENIED", true);
            if (messageObj != null)
            {
                var msg = string.Format(messageObj.MessageFormat);
                return msg;
            }

            return "Access denied. Please contact your System Administrator";

        }

        public static string SelectPublicServiceGrade(IRepositoryFactory factory = null)
        {
            factory = factory ?? new RepositoryFactory();
            factory.RegisterAllDependencies();
            var msgSrv = MessageFactory.GetMessageService(factory);

            var messageObj = msgSrv.GetMessageByCode("ERR-SELECT-GRADE", true);
            if (messageObj != null)
            {
                var msg = string.Format(messageObj.MessageFormat);
                return msg;
            }

            return "Error. Please select a Public Service grade/classification.";
        }


        public static string SelectTeachingServiceGrade(IRepositoryFactory factory = null)
        {
            factory = factory ?? new RepositoryFactory();
            factory.RegisterAllDependencies();
            var msgSrv = MessageFactory.GetMessageService(factory);

            var messageObj = msgSrv.GetMessageByCode("ERR-SELECT-TEACHING-GRADE", true);
            if (messageObj != null)
            {
                var msg = string.Format(messageObj.MessageFormat);
                return msg;
            }

            return "Error. Please select a Teaching Service grade/classification";
        }


        public static string PositionEndDateRequired(IRepositoryFactory factory = null)
        {
            factory = factory ?? new RepositoryFactory();
            factory.RegisterAllDependencies();
            var msgSrv = MessageFactory.GetMessageService(factory);

            var messageObj = msgSrv.GetMessageByCode("ERR-POSITION-END-DATE", true);
            if (messageObj != null)
            {
                var msg = string.Format(messageObj.MessageFormat);
                return msg;
            }

            return "Position end date is required for temporary positions";
        }

        public static string NoModifyPositionPermission(string unitName, IRepositoryFactory factory = null)
        {
            factory = factory ?? new RepositoryFactory();
            factory.RegisterAllDependencies();
            var msgSrv = MessageFactory.GetMessageService(factory);
            if (string.IsNullOrEmpty(unitName))
            {
                unitName = "that";
            }
            var messageObj = msgSrv.GetMessageByCode("ERR-USER-PERMIT-MODIFY-TEAM-POSITION", true);
            if (messageObj != null)
            {
                var msg = string.Format(messageObj.MessageFormat, unitName);
                return msg;
            }

            return $"You do not have permission to modify {unitName} team position";

        }


        public static string PositionNumberExists(string positionNum, IRepositoryFactory factory = null)
        {
            factory = factory ?? new RepositoryFactory();
            factory.RegisterAllDependencies();
            var msgSrv = MessageFactory.GetMessageService(factory);
            if (string.IsNullOrEmpty(positionNum))
            {
                positionNum = "entered";
            }
            var messageObj = msgSrv.GetMessageByCode("ERR-POSITION-NUM-EXISTS", true);
            if (messageObj != null)
            {
                var msg = string.Format(messageObj.MessageFormat, positionNum);
                return msg;
            }

            return $"The position number {positionNum} already exists.";

        }


        public static string ErrorOccured(IRepositoryFactory factory = null)
        {
            factory = factory ?? new RepositoryFactory();
            factory.RegisterAllDependencies();
            var msgSrv = MessageFactory.GetMessageService(factory);

            var messageObj = msgSrv.GetMessageByCode("ERR-ERROR-HAS-OCCURED", true);
            if (messageObj != null)
            {
                var msg = string.Format(messageObj.MessageFormat);
                return msg;
            }

            return "An error has occurred.";
        }



    }
}

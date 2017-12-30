using System.Collections.Generic;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices
{
    public class MessageService
    {
        public MessageService(List<SysMessage> messages)
        {
            MessagesList = messages;
        }

        public SysMessage GetMessageByCode(string code,bool trimTextAndContainers=true)
        {
            var sysMessage=MessagesList.SingleOrDefault(msg => msg.Code == code);
            if (sysMessage!=null&&trimTextAndContainers)
            {
                if (!string.IsNullOrWhiteSpace(sysMessage.MessageFormat))
                    sysMessage.MessageFormat = TrimTextAndContainers(sysMessage.MessageFormat);
                if(!string.IsNullOrWhiteSpace(sysMessage.MessageHint))
                    sysMessage.MessageHint = TrimTextAndContainers(sysMessage.MessageHint);
                }
            return sysMessage;
        }

        /// <summary>
        /// Trim Paragraph and divs
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string TrimTextAndContainers(string text)
        {
            return text.Trim().Replace("<p>", "").Replace("</p>", "").Replace("<div>", "").Replace("</div>", "");
        }
        public List<SysMessage> SearchMessages(string search,bool messageFormatOnly=false)
        {
            return MessagesList.Where(msg => msg.MessageHint.ToLower().Contains(search.ToLower())).ToList();
        }
        public List<SysMessage> MessagesList { get; }
    }
}
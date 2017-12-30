using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices
    {
    public class MessageFactory
        {
       public static MessageService GetMessageService(IRepositoryFactory factory = null,bool fromCache = true)
       {
           var  list = AppCacheHelper.GetResult<List<SysMessage>>(Enums.CacheRegion.Messages.ToString()) ??
                       new ServiceRepository(factory).SysMessageRepository().List().ToList();
            AppCacheHelper.Cache(Enums.CacheRegion.Messages.ToString(),list);
            return new MessageService(list);
            }
        public static void ExpireCache()
            {
            AppCacheHelper.Expire(Enums.CacheRegion.Messages);
            }
        
       
        }
    }


using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories 
{
    public partial class SysMessageRepository : BaseRepository<SysMessage> 
    {
        public SysMessageRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<SysMessage> List()
        {                       
            return base.List()
                    .Include(ent=>ent.SysMsgCategory) 
                    .OrderBy(ent=>ent.Code);
        }

        public override void Insert(SysMessage entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(SysMessage entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        public SysMessage GetMessageByCode(string code)
            {
            return List().SingleOrDefault(msg => msg.Code == code);
            }


        public IQueryable<SysMessage> FilterSysMessages(IQueryable<SysMessage> sysMessages, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredSysMessage = sysMessages.Where(ent => 
                    (!string.IsNullOrEmpty(ent.Code) && ent.Code.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.MessageFormat) && ent.MessageFormat.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.SysMsgCategory.MsgCategoryName) && ent.SysMsgCategory.MsgCategoryName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.MessageHint) && ent.MessageHint.ToLower().Contains(searchWord))
                    
);

            return filteredSysMessage.OrderBy(e => e.Code);
        }

        
    }
}




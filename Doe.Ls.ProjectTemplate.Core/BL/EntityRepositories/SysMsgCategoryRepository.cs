

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
    public partial class SysMsgCategoryRepository : BaseRepository<SysMsgCategory> 
    {
        public SysMsgCategoryRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<SysMsgCategory> List()
        {                       
            return base.List()
                    .Include(ent=>ent.SysMessages) 
                    .OrderBy(ent=>ent.MsgCategoryId);
        }

        public override void Insert(SysMsgCategory entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(SysMsgCategory entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<SysMsgCategory> FilterSysMsgCategorys(IQueryable<SysMsgCategory> sysMsgCategorys, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredSysMsgCategory = sysMsgCategorys.Where(ent => 
                    ent.MsgCategoryId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.MsgCategoryName) && ent.MsgCategoryName.ToLower().Contains(searchWord))
);

            return filteredSysMsgCategory.OrderBy(e => e.MsgCategoryId);
        }
    }
}




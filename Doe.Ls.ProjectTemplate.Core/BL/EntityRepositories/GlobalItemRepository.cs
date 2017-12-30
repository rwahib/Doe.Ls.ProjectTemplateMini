

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
    public partial class GlobalItemRepository : BaseRepository<GlobalItem> 
    {
        public GlobalItemRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<GlobalItem> List()
        {                       
            return base.List()
                    .OrderBy(ent=>ent.ItemCode);
        }

        public IEnumerable<GlobalItem> LoadAll()
        {
             return List().ToList();
        }

        public GlobalItem GetGlobalItemByCode(string code)
        {
            return List().SingleOrDefault(i => i.ItemCode.ToLower()== code.ToLower());
        }
        public GlobalItem GetGlobalItemByCode(IEnumerable<GlobalItem> list, string code)
            {
            return list.SingleOrDefault(i => i.ItemCode.ToLower() == code.ToLower());
            }

        public override void Insert(GlobalItem entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(GlobalItem entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        

        public IQueryable<GlobalItem> FilterItems(IQueryable<GlobalItem> items, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredItem = items.Where(ent => 
                    (!string.IsNullOrEmpty(ent.ItemCode) && ent.ItemCode.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.ItemName) && ent.ItemName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.ItemDescription) && ent.ItemDescription.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.ItemContent) && ent.ItemContent.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.LastupdatedBy) && ent.LastupdatedBy.ToLower().Contains(searchWord))
);

            return filteredItem.OrderBy(e => e.ItemCode);
        }
    }
}




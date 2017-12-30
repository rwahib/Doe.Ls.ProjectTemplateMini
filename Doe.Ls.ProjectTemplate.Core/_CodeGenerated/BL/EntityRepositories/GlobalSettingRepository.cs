

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
    public partial class GlobalSettingRepository : BaseRepository<GlobalSetting> 
    {
        public GlobalSettingRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<GlobalSetting> List()
        {                       
            return base.List()
                    .OrderBy(ent=>ent.SettingsKey);
        }

        public override void Insert(GlobalSetting entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(GlobalSetting entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<GlobalSetting> FilterGlobalSettings(IQueryable<GlobalSetting> globalSettings, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredGlobalSetting = globalSettings.Where(ent => 
                    ent.SettingsKey.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.PropertyCode) && ent.PropertyCode.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.PropertyValue) && ent.PropertyValue.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.EntityContextCode) && ent.EntityContextCode.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.EntityContextValue) && ent.EntityContextValue.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Tag) && ent.Tag.ToLower().Contains(searchWord))
);

            return filteredGlobalSetting.OrderBy(e => e.SettingsKey);
        }
    }
}




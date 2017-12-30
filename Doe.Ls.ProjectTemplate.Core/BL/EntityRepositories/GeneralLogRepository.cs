

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories 
{
    public partial class GeneralLogRepository : BaseRepository<GeneralLog> 
    {
        public GeneralLogRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<GeneralLog> List()
        {                       
            return base.List().Include(l=>l.SysRole)
                    .OrderBy(ent=>ent.LogId);
        }

        public override void Insert(GeneralLog entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public IQueryable<GeneralLog> FilterGeneralLogs(IQueryable<GeneralLog> generalLogs, GeneralLogArgument arg)
        {

            var filteredGeneralLog = generalLogs;

            if(arg.FromDate.HasValue || arg.ToDate.HasValue)
                {
                var fromDate = arg.FromDate.HasValue ? arg.FromDate.Value : DateTime.MinValue;
                var toDate = arg.ToDate.HasValue ? arg.ToDate.Value : DateTime.MaxValue;
                filteredGeneralLog = filteredGeneralLog.Where(l => l.CreationDate >= fromDate && l.CreationDate <= toDate);
                };
            if(!string.IsNullOrWhiteSpace(arg.LogAction))
                {
                filteredGeneralLog = filteredGeneralLog.Where(l => l.Action==arg.LogAction);
                };
            if(!string.IsNullOrWhiteSpace(arg.sSearch)) {
                filteredGeneralLog=filteredGeneralLog.Where(ent =>                         
                        (!string.IsNullOrEmpty(ent.Context) && ent.Context.ToLower().Contains(arg.sSearch))
                        || (!string.IsNullOrEmpty(ent.Username) && ent.Username.ToLower().Contains(arg.sSearch))                        
                        || (!string.IsNullOrEmpty(ent.Note) && ent.Note.ToLower().Contains(arg.sSearch)));
                    }
;

            return filteredGeneralLog;
        }

        public void Log(Enums.LogActions action, string context, string note = null)
            {
            var user = this.GetCurrentUser() as UserInfoExtension;
            if (user != null)
            {
                Log(action, context, user.UserName, user.CurrentRoleEnum, note);
            }
            else
            {
                Log(action, context, "Guest", Enums.UserRole.Guest, note);
                }

            }
        
        public void Log(Enums.LogActions action, string context, string userName, Enums.UserRole userRole, string note = null)
            {

            //Thread.Sleep(5000);

            var logItem = new GeneralLog
                {
                Action = action.GetDescription(),
                Context = context,
                Username = userName,
                RoleId = userRole.ToInteger(),
                Note = note,
                CreationDate = DateTime.Now,

                };
            this.Insert(logItem);
            }
        
        }
}




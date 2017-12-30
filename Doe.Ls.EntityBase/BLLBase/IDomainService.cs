using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;

namespace Doe.Ls.EntityBase.BLLBase
{
    public interface IDomainService : IDisposable
    {
        ILoggerService LoggerService { get; set; }
        IRepositoryFactory RepositoryFactory { get; set; }
        ISessionService SessionService { get; set; }
    }
}
using System;
using System.Data.Entity;

namespace Doe.Ls.EntityBase.RepositoryBase
{
    using IsolationLevel = System.Data.IsolationLevel;
    public interface IUnitOfWork : IDisposable
    {
        DbContext DbContext { get; }
        Guid StratTransction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void CommitTransaction(Guid token);
        void RollbackTransaction(Guid token);
        bool IsInTransaction(Guid token);
    }
}

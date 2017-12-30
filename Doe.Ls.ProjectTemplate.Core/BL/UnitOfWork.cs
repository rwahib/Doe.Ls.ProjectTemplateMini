using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Doe.Ls.EntityBase.RepositoryBase;
using IsolationLevel = System.Data.IsolationLevel;

namespace Doe.Ls.ProjectTemplate.Core.BL
{
    public class UnitOfWork : IUnitOfWork
    {

        protected  Data.SampleProjectTemplateEntities Db;
        private readonly Dictionary<string, Guid> _tokensGuids = new Dictionary<string, Guid>();
        private DbContextTransaction _transaction = null;
        public UnitOfWork()
        {
            Db = new Data.SampleProjectTemplateEntities();
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public bool IsInTransaction(Guid token)
        {
            return this.DbContext.Database.CurrentTransaction != null && _tokensGuids.Values.Contains(token);
        }

        public DbContext DbContext
        {
            get { return Db; }
        }

        public Guid StratTransction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var token = Guid.NewGuid();

            if (this.DbContext.Database.CurrentTransaction == null)
            {
                _transaction = this.DbContext.Database.BeginTransaction();
                this._tokensGuids.Add(token.ToString(), token);
            }
            return token;
        }



        public void CommitTransaction(Guid token)
        {
            if (IsInTransaction(token))
            {
                _transaction.Commit();
                RemoveTransaction(token);
            }
        }

        public void RollbackTransaction(Guid token)
        {
            if (IsInTransaction(token))
            {
                _transaction.Rollback();
                RemoveTransaction(token);
            }

        }

        private void RemoveTransaction(Guid token)
        {
            _tokensGuids.Remove(token.ToString());
            _transaction.Dispose();
            _transaction = null;
        }
    }
}
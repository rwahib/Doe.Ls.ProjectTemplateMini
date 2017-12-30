using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.SessionService;

namespace Doe.Ls.EntityBase.RepositoryBase
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Returns a raw collection, can be used with where, sort, all of other operations.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> List();

        T GetEntityByKey(object[] primaryKeys);

        bool Exists(object[] primaryKeys);
        void InsertEntity(T entity, bool saveChanges);
        void Insert(T entity);
        void Update(T entity, bool refresh);
        void UpdateEntity(T entity, bool refresh, bool saveChanges);
        void Refresh(T entity);
        void SaveChanges();
        void Delete(T entity);
        void DeleteEntity(T entity, bool saveChanges);


        void Refresh(IEnumerable<T> entities);

        int ExecuteSqlCommand(string sql, params SqlParameter[] parameters);

        ILoggerService LoggerService { get; }
        IUnitOfWork UnitOfWork { get; }
        ISessionService SessionService { get; }
    }
}
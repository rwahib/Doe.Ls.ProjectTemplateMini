using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.SessionService;

using Unity.Attributes;

namespace Doe.Ls.EntityBase.RepositoryBase
{
    /// <summary>
    /// Base class for all SQL based service classes
    /// </summary>
    /// <typeparam name="T">The domain object type</typeparam>    
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;
        private readonly ILoggerService _loggerService;
        protected readonly DbSet<T> _dbSet;


        public IUnitOfWork UnitOfWork { get { return _unitOfWork; } }

        public UserInfo GetCurrentUser()
        {
            var user = this.SessionService.ReadFromSession<UserInfo>(Cnt.CurrentUserKey) ?? new UserInfo
            {
                UserName = "System"
            };
            return user;
        }


        public ILoggerService LoggerService
        {
            get { return _loggerService; }
        }

        public ISessionService SessionService
        {
            get { return _sessionService; }
        }


        public BaseRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService)
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
            if (loggerService == null) throw new ArgumentNullException(nameof(loggerService));
            if (sessionService == null) throw new ArgumentNullException(nameof(sessionService));

            _unitOfWork = unitOfWork;
            _loggerService = loggerService;
            _sessionService = sessionService;
            this._dbSet = _unitOfWork.DbContext.Set<T>();
        }

        /// <summary>
        /// Returns a raw collection, can be used with where, sort, all of other operations.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> List()
        {
            return _dbSet.AsQueryable();
        }

        public virtual IQueryable<T> Where(string whereClause)
        {
            return List().Where(whereClause);
        }

        /// <summary>
        /// Use single of default better than Find, so it can resolve all includes as well
        /// </summary>        
        public virtual T GetEntityByKey(params object[] primaryKeys)
        {
            var dbResult = _dbSet.Find(primaryKeys);
            if (dbResult != null) Refresh(dbResult);
            return dbResult;
        }

        public bool Exists(params object[] primaryKeys)
        {
            return _dbSet.Find(primaryKeys) != null;
        }

        public virtual void InsertEntity(T entity, bool saveChanges)
        {
            _dbSet.Add(entity);
            if (saveChanges) SaveChanges();
        }
        public virtual void Insert(T entity)
        {
            InsertEntity(entity, true);
        }
        public void LoadNavigationProperty<TElement>(T entity, Expression<Func<T, ICollection<TElement>>> navigationProperty) where TElement : class
        {
            if (_unitOfWork.DbContext.Entry(entity).State == EntityState.Detached) return;

            var navigation = _unitOfWork.DbContext.Entry(entity).Collection(navigationProperty);
            if (!navigation.IsLoaded)
            {
                navigation.Load();
            }
        }
        public void LoadNavigationProperty(T entity, string navigationProperty)
        {
            if (_unitOfWork.DbContext.Entry(entity).State == EntityState.Detached) return;
            var navigation = _unitOfWork.DbContext.Entry(entity).Collection(navigationProperty);
            if (!navigation.IsLoaded)
            {
                navigation.Load();
            }

        }
        public virtual void UpdateEntity(T entity, bool refresh, bool saveChanges)
        {

            _dbSet.Attach(entity);

            _unitOfWork.DbContext.Entry(entity).State = EntityState.Modified;
            if (saveChanges) SaveChanges();
            if (!refresh) Refresh(entity);
        }

        public virtual void Update(T entity, bool refresh = true)
        {

            UpdateEntity(entity, true, refresh);
        }
        public void Refresh(T entity)
        {
            _unitOfWork.DbContext.Entry(entity).Reload();

        }

        public void Refresh(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _unitOfWork.DbContext.Entry(entity).Reload();
            }

        }

        public virtual void DeleteEntity(T entity, bool saveChanges)
        {
            T existingEntity = entity;
            if (_unitOfWork.DbContext.Entry(entity).State == EntityState.Detached)
            {
                try
                {
                    _dbSet.Attach(existingEntity);
                }
                catch // if because the same entity is there 
                {
                    existingEntity = GetEntityByEntityKey(entity);
                    _dbSet.Attach(existingEntity);

                }

            }
            _dbSet.Remove(existingEntity);

            if (saveChanges) { SaveChanges(); }

        }
        public virtual void Delete(T entity)
        {
            DeleteEntity(entity, true);

        }
        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            SaveChanges();
        }

        public void DeleteFromDb(T entity, bool isUnitTest)
        {
            if (!isUnitTest)
            {
                throw new InvalidOperationException("This method is permissible only from Unit Test");
            }

            var connection = UnitOfWork.DbContext.Database.Connection.ConnectionString.ToLower();
            if (!connection.Contains("dbdev"))
            {
                throw new InvalidOperationException("This database is not DEV database");
            }

            Delete(entity);

        }


        public int ExecuteSqlCommand(string sql, params SqlParameter[] parameters)
        {

            return this._unitOfWork.DbContext.Database.ExecuteSqlCommand(sql, parameters);

        }
        public int ExecuteFunction(string functionName, params ObjectParameter[] parameters)
        {

            return ((IObjectContextAdapter)this._unitOfWork.DbContext).ObjectContext.ExecuteFunction(functionName, parameters);

        }
        public DbRawSqlQuery<T> SqlQuery(string sql)
        {

            return this._unitOfWork.DbContext.Database.SqlQuery<T>(sql);

        }

        public IEnumerable<DbValidationError> GetValidationErrors(ModelStateDictionary modelState)
        {

            var errors = new List<DbValidationError>();
            foreach (var state in modelState)
            {
                foreach (var err in state.Value.Errors)
                {
                    string error;
                    error = err.ErrorMessage;
                    if (string.IsNullOrWhiteSpace(error)) error = err.Exception.Message;

                    errors.Add(new DbValidationError(state.Key, error));

                }
            }
            return errors;
        }
        public IEnumerable<DbValidationError> GetBackendValidationErrors()
        {

            return this.UnitOfWork.DbContext.GetValidationErrors().SelectMany(err => err.ValidationErrors);

        }


        public List<DbValidationError> ValidateEntity(T entity)
        {
            var errors = new List<DbValidationError>();
            if (entity.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), false).Any())
            {

                var metadataClassAttribute =
                    (entity.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), false).First() as
                        MetadataTypeAttribute);

                foreach (var pInfo in metadataClassAttribute.MetadataClassType.GetProperties())
                {

                    if (pInfo.GetCustomAttributes(typeof(ValidationAttribute), true).Any())
                    {

                        foreach (ValidationAttribute validationAttribute in pInfo.GetCustomAttributes(typeof(ValidationAttribute), true))
                        {

                            if (entity.GetType().GetProperties().Any(s => s.Name == pInfo.Name))
                            {
                                var destinationProperty =
                                    entity.GetType().GetProperties().Single(s => s.Name == pInfo.Name);

                                if (!validationAttribute.IsValid(destinationProperty.GetValue(entity, null)))
                                {
                                    errors.Add(new DbValidationError(pInfo.Name, validationAttribute.ErrorMessage));
                                }
                            }
                        }
                    }
                }
            }


            return errors;
        }
        public virtual T GetEntityByEntityKey(T entity)
        {
            var entityType = T4Helper.GetEntityType(typeof(T).Name, this.UnitOfWork.DbContext);
            var keysProperties = T4Helper.GetKeys(entityType);
            var keysObjects = GetKeysValue(keysProperties, entity);

            return GetEntityByKey(keysObjects);
        }

        private object[] GetKeysValue(List<EdmProperty> keysProperties, T entity)
        {
            var keysObjects = new List<object>();
            foreach (var pInfo in entity.GetType().GetProperties())
            {
                if (keysProperties.Any(p => p.Name == pInfo.Name))
                {
                    keysObjects.Add(pInfo.GetValue(entity, null));

                }
            }
            return keysObjects.ToArray();
        }

        public void SetPropertyValuesFrom(ref T source, T destination, bool ignoreNullValues = true, string[] execludeNames = null)
        {
            object s = source;
            CustomConverter.UpdateProperties(s, destination, ignoreNullValues, execludeNames);
        }

        public void SetPropertyValuesFrom(ref object source, object destination, bool ignoreNullValues = true)
        {
            CustomConverter.UpdateProperties(source, destination, ignoreNullValues);
        }

        public IEnumerable<DbValidationError> GetServerValidationErrors()
        {

            return this.UnitOfWork.DbContext.GetValidationErrors().SelectMany(err => err.ValidationErrors);

        }
        public virtual int GetNewKey()
        {
            return GetMaxKeyValue() + 1;
        }

        public virtual int GetNewKey10S()
        {
            var newKey = GetNewKey();
            return (newKey / 10) * 10 + 10;
        }

        public int GetMaxKeyValue()
        {
            if (!this.List().Any())
                return 1;
            var entityType = T4Helper.GetEntityType(typeof(T).Name, this.UnitOfWork.DbContext);

            var keysProperties = T4Helper.GetKeys(entityType);

            if (keysProperties.Count != 1)
            {
                throw new InvalidOperationException($"Entity {entityType.Name} has not a single primary key");
            }
            var key = keysProperties.SingleOrDefault();

            return GetMaxValue(key.Name);

        }

        public int GetMaxValue(string property)
        {
            if (!this.List().Any())
                return 1;
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.Property(parameter, property);
            var lambda = Expression.Lambda<Func<T, int>>(body, parameter);
            var result = List().Max(lambda);
            return result;

        }

        public virtual void SaveChanges()
        {
            try
            {
                this._unitOfWork.DbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                if (this.GetServerValidationErrors().Any())
                {
                    var sb = new StringBuilder();
                    foreach (var err in GetServerValidationErrors())
                    {
                        sb.Append($"property {err.PropertyName} has error: {err.ErrorMessage + ".\n\r"}");
                    }

                    throw new Exception(sb.ToString(), innerException: exception);

                }

                LoggerService.Log(exception);
                LoggerService.SendMail(exception);
                var propName = "id";
                if (exception.ToString().Contains("duplicate key"))
                {
                    if (exception is DbUpdateException)
                    {
                        var dbException = exception as DbUpdateException;
                        var result = dbException.Entries.Select(entry => entry.Entity).ToArray();
                        var entity = result.First();

                        var helper = new EdmMetadataHelper(this._unitOfWork.DbContext);
                        var entityType =
                            helper.GetEntityList(DataSpace.SSpace).First(ent => ent.Name == entity.GetType().Name);

                        var key = T4Helper.GetKeys(entityType).FirstOrDefault();
                        if (key != null) propName = key.Name;
                    }
                    if (_dbSet != null && this._dbSet.Local != null && this._dbSet.Local.Count >= 1)
                        ClearCache();
                    throw new Exception(message: "This " + propName.Wordify() + " already exists in database", innerException: exception);

                }
                if (_dbSet != null && this._dbSet.Local != null && this._dbSet.Local.Count >= 1)
                    ClearCache();
                throw;
            }
        }


        private void ClearCache()
        {

            var entries = _dbSet.Local;
            foreach (var entry in entries.ToArray())
            {
                var state = _unitOfWork.DbContext.Entry(entry).State;
                if (state == EntityState.Added) _dbSet.Local.Remove(entry);
                if (state == EntityState.Modified) this.Refresh(entry);
            }

        }

        [Unity.Attributes.Dependency]
        public IRepositoryFactory RepositoryFactory { get; set; }
    }
}

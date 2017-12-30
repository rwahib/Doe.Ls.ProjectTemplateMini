﻿
namespace Doe.Ls.EntityBase.RepositoryBase
{
    /// <summary>
    /// Marks a service that can be used to access a database
    /// </summary>
    /// <typeparam name="T">The domain object return type</typeparam>
    public interface IDbRepository<T> : IRepository<T>
    {
        
    }
}
using System;
using System.Linq.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Doe.Ls.EntityBase.Models;


namespace Doe.Ls.EntityBase.Helper
    {
    public static class CustomOrderBy
        {
        /**************make sorting of columns generalized way *******************/

        public static IQueryable<T> CustomSort<T>(IQueryable<T> source, JQueryDataTableParamModel param)
            {
            var propertyName = !string.IsNullOrEmpty(param.sColumns)
                ? param.sColumns.Split(',')[param.iSortCol_0]
                : String.Empty;


            bool isAsc = param.sSortDir_0 == "asc";
            if(null == source) throw new ArgumentNullException(nameof(source));
            if(string.IsNullOrEmpty(propertyName)) return source;

            var p = Expression.Parameter(typeof(T), "p");
            var prop = CreateExpression(typeof(T), propertyName, p);
            var keySelector = Expression.Lambda(prop, p);
            // p => p.Property

            var pSource = Expression.Parameter(typeof(IQueryable<T>), "source");
            var orderBy = Expression.Call(typeof(Queryable),
                (isAsc ? "OrderBy" : "OrderByDescending"),
                new[] { typeof(T), prop.Type },
                pSource, keySelector);

            var sorter = Expression.Lambda<Func<IQueryable<T>, IQueryable<T>>>(orderBy, pSource);


            return sorter.Compile()(source);
            }

        static Expression CreateExpression(Type type, string propertyName, ParameterExpression param)
            {
            return propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);
            }

        public static IQueryable<T> CustomSort<T>(IQueryable<T> source, string columnNames, bool desc)
            {
            var orderBy = desc ? columnNames + " desc" : columnNames;
            
            return source.OrderBy(orderBy);
            }

        /// 
        /// <param name="columnNames">A, B, C or A asc, B desc</param>
        
        public static IQueryable<T> CustomSort<T>(IQueryable<T> source, string columnNames)
            {
            
            return source.OrderBy(columnNames);
            }

        }
    }

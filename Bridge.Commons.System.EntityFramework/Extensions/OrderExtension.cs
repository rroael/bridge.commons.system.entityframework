using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Bridge.Commons.System.Contracts;
using Bridge.Commons.System.Enums;

namespace Bridge.Commons.System.EntityFramework.Extensions
{
    /// <summary>
    ///     Extensão de ordenação
    /// </summary>
    public static class OrderExtension
    {
        #region Métodos de ordenação

        /// <summary>
        ///     Ordenado por
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fieldName"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string fieldName)
            where TEntity : class
        {
            var resultExp = GenerateMethodCall(source, "OrderBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        /// <summary>
        ///     Ordenação descendente
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fieldName"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(this IQueryable<TEntity> source,
            string fieldName)
            where TEntity : class
        {
            var resultExp = GenerateMethodCall(source, "OrderByDescending", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        /// <summary>
        ///     Cria uma próxima fase de ordenação
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fieldName"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IOrderedQueryable<TEntity> source,
            string fieldName)
            where TEntity : class
        {
            var resultExp = GenerateMethodCall(source, "ThenBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        /// <summary>
        ///     Faz uma próxima ordenação por descendencia
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fieldName"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(this IOrderedQueryable<TEntity> source,
            string fieldName)
            where TEntity : class
        {
            var resultExp = GenerateMethodCall(source, "ThenByDescending", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        /// <summary>
        ///     Ordena usando Sort Expression
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sortExpression"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static IOrderedQueryable<TEntity> OrderUsingSortExpression<TEntity>(this IQueryable<TEntity> source,
            string sortExpression)
            where TEntity : class
        {
            var orderFields = sortExpression.Split(',');
            IOrderedQueryable<TEntity> result = null;
            for (var currentFieldIndex = 0; currentFieldIndex < orderFields.Length; currentFieldIndex++)
            {
                var expressionPart = orderFields[currentFieldIndex].Trim().Split(' ');
                var sortField = expressionPart[0];
                var sortDescending = expressionPart.Length == 2 &&
                                     expressionPart[1].Equals("DESC", StringComparison.OrdinalIgnoreCase);
                if (sortDescending)
                    result = currentFieldIndex == 0
                        ? source.OrderByDescending(sortField)
                        : result.ThenByDescending(sortField);
                else
                    result = currentFieldIndex == 0 ? source.OrderBy(sortField) : result.ThenBy(sortField);
            }

            return result;
        }

        /// <summary>
        ///     Ordenação por paginação
        /// </summary>
        /// <param name="query">Objeto IQueryable</param>
        /// <param name="pagination">Paginação</param>
        /// <param name="defaultField">Campo padrão para ordenação</param>
        /// <param name="defaultSort">Ordenação ASCENDING ou DESCENDING</param>
        /// <typeparam name="TEntity">Entidade</typeparam>
        /// <returns></returns>
        public static IQueryable<TEntity> Sort<TEntity>(this IQueryable<TEntity> query, IPagination pagination,
            string defaultField = null, ESortType defaultSort = ESortType.ASCENDING) where TEntity : class
        {
            if (pagination.SortField != null)
                query = pagination.Order == ESortType.ASCENDING
                    ? query.OrderBy(pagination.SortField)
                    : query.OrderByDescending(pagination.SortField);
            else if (!string.IsNullOrEmpty(defaultField))
                query = defaultSort == ESortType.ASCENDING
                    ? query.OrderBy(defaultField)
                    : query.OrderByDescending(defaultField);

            return query;
        }

        #endregion


        #region Private expression tree helpers

        private static LambdaExpression GenerateSelector<TEntity>(string propertyName, out Type resultType)
            where TEntity : class
        {
            // Create a parameter to pass into the Lambda expression (Entity => Entity.OrderByField).
            var parameter = Expression.Parameter(typeof(TEntity), "Entity");

            //  create the selector part, but support child properties
            PropertyInfo property;
            Expression propertyAccess;
            if (propertyName.Contains('.'))
            {
                // support to be sorted on child fields.
                var childProperties = propertyName.Split('.');
                property = typeof(TEntity).GetProperty(childProperties[0],
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
                for (var i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i],
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase);
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = typeof(TEntity).GetProperty(propertyName,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }

            resultType = property.PropertyType;
            // Create the order by expression.
            return Expression.Lambda(propertyAccess, parameter);
        }

        private static MethodCallExpression GenerateMethodCall<TEntity>(IQueryable<TEntity> source, string methodName,
            string fieldName) where TEntity : class
        {
            var type = typeof(TEntity);
            Type selectorResultType;
            var selector = GenerateSelector<TEntity>(fieldName, out selectorResultType);
            var resultExp = Expression.Call(typeof(Queryable), methodName,
                new[] { type, selectorResultType },
                source.Expression, Expression.Quote(selector));
            return resultExp;
        }

        #endregion
    }
}
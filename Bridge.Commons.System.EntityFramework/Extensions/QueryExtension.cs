using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bridge.Commons.System.Contracts;
using Bridge.Commons.System.Contracts.Mappers;
using Bridge.Commons.System.Models.Results;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Commons.System.EntityFramework.Extensions
{
    /// <summary>
    ///     Extensão de query
    /// </summary>
    public static class QueryExtension
    {
        /// <summary>
        ///     Buscar lista (assíncrono)
        /// </summary>
        /// <param name="query"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static async Task<IList<TResult>> GetListAsync<TEntity, TResult>(this IQueryable<TEntity> query)
            where TEntity : class, IToObjectMapper<TResult>
            where TResult : class
        {
            IList<TEntity> list = await query.AsNoTracking().ToListAsync();

            return new List<TResult>(list.Select(x => x.MapTo()));
        }

        /// <summary>
        ///     Busca lista paginada (assíncrono)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pagination"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static async Task<PaginatedList<TResult>> GetPaginatedListAsync<TEntity, TResult>(
            this IQueryable<TEntity> query, IPagination pagination)
            where TEntity : class, IToObjectMapper<TResult>
            where TResult : class
        {
            int pageCount = 0, totalCount = 0;

            totalCount = await query.AsNoTracking().CountAsync();
            pageCount = pagination.GetPageCount(totalCount);

            IList<TEntity> list =
                await query.AsNoTracking().Skip(pagination.Skip).Take(pagination.PageSize).ToListAsync();

            return new PaginatedList<TResult>(list.Select(x => x.MapTo()), pagination.Page, pagination.PageSize,
                pageCount, totalCount);
        }

        /// <summary>
        ///     Buscar
        /// </summary>
        /// <param name="query"></param>
        /// <param name="id"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <returns></returns>
        public static async Task<TEntity> Get<TEntity, TId>(this IQueryable<TEntity> query, TId id)
            where TEntity : class, IIdentifiable<TId>
            where TId : IConvertible
        {
            return await query.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }


        /// <summary>
        ///     Buscar assíncrono
        /// </summary>
        /// <param name="query"></param>
        /// <param name="id"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <returns></returns>
        public static async Task<TEntity> GetAsync<TEntity, TId>(this IQueryable<TEntity> query, TId id)
            where TEntity : class, IIdentifiable<TId>
            where TId : IConvertible
        {
            return await query.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}
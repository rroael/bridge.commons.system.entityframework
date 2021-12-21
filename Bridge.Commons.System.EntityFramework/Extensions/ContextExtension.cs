using System;
using System.Linq;
using System.Threading.Tasks;
using Bridge.Commons.System.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Commons.System.EntityFramework.Extensions
{
    /// <summary>
    ///     Extensão do contexto
    /// </summary>
    public static class ContextExtension
    {
        /// <summary>
        ///     Buscar em cache
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <returns></returns>
        public static TEntity GetCached<TEntity, TId>(this DbContext context, TEntity entity)
            where TEntity : class, IIdentifiable<TId>
            where TId : IConvertible
        {
            return context.Set<TEntity>().Local.FirstOrDefault(x => x.Id.Equals(entity.Id));
        }

        /// <summary>
        ///     Buscar
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <returns></returns>
        public static TEntity Get<TEntity, TId>(this DbContext context, TId id)
            where TEntity : class, IIdentifiable<TId>
            where TId : IConvertible
        {
            return context.Set<TEntity>().FirstOrDefault(x => x.Id.Equals(id));
        }

        /// <summary>
        ///     Buscar assíncrono
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <returns></returns>
        public static async Task<TEntity> GetAsync<TEntity, TId>(this DbContext context, TId id)
            where TEntity : class, IIdentifiable<TId>
            where TId : IConvertible
        {
            return await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}
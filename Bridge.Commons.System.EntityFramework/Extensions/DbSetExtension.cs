using System;
using System.Linq;
using System.Threading.Tasks;
using Bridge.Commons.System.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Commons.System.EntityFramework.Extensions
{
    /// <summary>
    ///     Extensão Entity Framework
    /// </summary>
    public static class DbSetExtension
    {
        #region Generic

        /// <summary>
        ///     Achar, criar ou atualizar (generic)
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TId"></typeparam>
        public static void FindCreateOrUpdate<TEntity, TId>(this DbSet<TEntity> dbSet, TEntity entity)
            where TEntity : class, IIdentifiable<TId>
            where TId : IComparable, IConvertible
        {
            var founded = dbSet.AsNoTracking().FirstOrDefault(x => entity.Id.Equals(x.Id));
            if (founded != null)
                dbSet.Update(entity);
            else
                dbSet.Add(entity);
        }

        /// <summary>
        ///     Achar, criar ou atualizar assíncrono (generic)
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <returns></returns>
        public static async Task FindCreateOrUpdateAsync<TEntity, TId>(this DbSet<TEntity> dbSet, TEntity entity)
            where TEntity : class, IIdentifiable<TId>
            where TId : IComparable, IConvertible
        {
            var founded = await dbSet.AsNoTracking().FirstOrDefaultAsync(x => entity.Id.Equals(x.Id));
            if (founded != null)
                dbSet.Update(entity);
            else
                await dbSet.AddAsync(entity);
        }

        #endregion


        #region Integer

        /// <summary>
        ///     Criar ou atualizar lista (int)
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entities"></param>
        /// <typeparam name="TEntity"></typeparam>
        public static void CreateOrUpdateList<TEntity>(this DbSet<TEntity> dbSet, params TEntity[] entities)
            where TEntity : class, IIdentifiable<int>
        {
            foreach (var entity in entities)
                CreateOrUpdate(dbSet, entity);
        }

        /// <summary>
        ///     Criar ou atualizar (int)
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        public static void CreateOrUpdate<TEntity>(this DbSet<TEntity> dbSet, TEntity entity)
            where TEntity : class, IIdentifiable<int>
        {
            if (entity.Id != 0)
                dbSet.Update(entity);
            else
                dbSet.Add(entity);
        }

        #endregion

        #region Long

        /// <summary>
        ///     Criar ou atualizar lista (long)
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entities"></param>
        /// <typeparam name="TEntity"></typeparam>
        public static void CreateOrUpdateListLong<TEntity>(this DbSet<TEntity> dbSet, params TEntity[] entities)
            where TEntity : class, IIdentifiable<long>
        {
            foreach (var entity in entities)
                CreateOrUpdateLong(dbSet, entity);
        }

        /// <summary>
        ///     Criar ou atualizar (long)
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        public static void CreateOrUpdateLong<TEntity>(this DbSet<TEntity> dbSet, TEntity entity)
            where TEntity : class, IIdentifiable<long>
        {
            if (entity.Id != 0)
                dbSet.Update(entity);
            else
                dbSet.Add(entity);
        }

        #endregion


        #region Short

        /// <summary>
        ///     Criar ou atualizar lista (short)
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entities"></param>
        /// <typeparam name="TEntity"></typeparam>
        public static void CreateOrUpdateListShort<TEntity>(this DbSet<TEntity> dbSet, params TEntity[] entities)
            where TEntity : class, IIdentifiable<short>
        {
            foreach (var entity in entities)
                CreateOrUpdateShort(dbSet, entity);
        }

        /// <summary>
        ///     Criar ou atualizar (short)
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        public static void CreateOrUpdateShort<TEntity>(this DbSet<TEntity> dbSet, TEntity entity)
            where TEntity : class, IIdentifiable<short>
        {
            if (entity.Id != 0)
                dbSet.Update(entity);
            else
                dbSet.Add(entity);
        }

        #endregion


        #region String

        /// <summary>
        ///     Criar ou atualizar lista (string)
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entities"></param>
        /// <typeparam name="TEntity"></typeparam>
        public static void CreateOrUpdateListString<TEntity>(this DbSet<TEntity> dbSet, params TEntity[] entities)
            where TEntity : class, IIdentifiable<string>
        {
            foreach (var entity in entities)
                CreateOrUpdateString(dbSet, entity);
        }

        /// <summary>
        ///     Criar ou atualizar (string)
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        public static void CreateOrUpdateString<TEntity>(this DbSet<TEntity> dbSet, TEntity entity)
            where TEntity : class, IIdentifiable<string>
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                dbSet.Update(entity);
            else
                dbSet.Add(entity);
        }

        #endregion
    }
}
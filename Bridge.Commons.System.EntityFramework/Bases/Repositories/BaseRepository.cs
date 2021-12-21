using System;
using System.Threading.Tasks;
using Bridge.Commons.System.Contracts;
using Bridge.Commons.System.EntityFramework.Bases.Contexts;
using Bridge.Commons.System.EntityFramework.Extensions;
using Bridge.Commons.System.Enums;
using Bridge.Commons.System.Exceptions;
using Bridge.Commons.System.Resources;

namespace Bridge.Commons.System.EntityFramework.Bases.Repositories
{
    /// <summary>
    ///     Base do repositório
    /// </summary>
    /// <typeparam name="TReadContext">Context de conexão de leitura</typeparam>
    /// <typeparam name="TWriteContext">Context de conexão de escrita</typeparam>
    /// <typeparam name="TEntity">Entidade</typeparam>
    /// <typeparam name="TIdentifiable">Tipo do identificador</typeparam>
    /// <typeparam name="TId">Tipo do identificador</typeparam>
    public abstract class BaseRepository<TReadContext, TWriteContext, TEntity, TIdentifiable, TId>
        : BaseNoIdRepository<TReadContext, TWriteContext, TEntity>
        where TReadContext : BaseReadOnlyContext
        where TWriteContext : BaseWriteContext
        where TEntity : class, IIdentifiable<TId>
        where TIdentifiable : IIdentifiable<TId>
        where TId : IConvertible
    {
        /// <summary>
        ///     Valida identificador
        /// </summary>
        /// <param name="identifiable"></param>
        /// <exception cref="RepositoryException"></exception>
        protected virtual void ValidateIdentifiable(TIdentifiable identifiable)
        {
            if (identifiable == null)
                throw new RepositoryException((int)EBaseError.FIELD_MUST_BE_FILLED,
                    string.Format(BaseErrors.FieldMustBeFilled, "Id"));
        }

        /// <summary>
        ///     Busca por identificador e valida
        /// </summary>
        /// <param name="identifiable"></param>
        /// <returns></returns>
        protected virtual async Task<TEntity> GetByIdentifiableAndValidateAsync(TIdentifiable identifiable)
        {
            ValidateIdentifiable(identifiable);

            var entity = await GetQueryable().GetAsync(identifiable.Id);

            ValidateEntity(entity);

            return entity;
        }

        /// <summary>
        ///     Busca por identificador
        /// </summary>
        /// <param name="identifiable"></param>
        /// <returns></returns>
        protected virtual async Task<TEntity> GetByIdentifiableAsync(TIdentifiable identifiable)
        {
            ValidateIdentifiable(identifiable);

            return await GetQueryable().GetAsync(identifiable.Id);
        }
    }
}
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Commons.System.Contracts.Filters;
using Bridge.Commons.System.EntityFramework.Bases.Contexts;
using Bridge.Commons.System.Enums;
using Bridge.Commons.System.Exceptions;
using Bridge.Commons.System.Resources;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Commons.System.EntityFramework.Bases.Repositories
{
    /// <summary>
    ///     Base do repositório para tabelas sem Id
    /// </summary>
    /// <typeparam name="TReadContext">Context de conexão de leitura</typeparam>
    /// <typeparam name="TWriteContext">Context de conexão de escrita</typeparam>
    /// <typeparam name="TEntity">Entidade</typeparam>
    public abstract class BaseNoIdRepository<TReadContext, TWriteContext, TEntity>
        where TReadContext : BaseReadOnlyContext
        where TWriteContext : BaseWriteContext
        where TEntity : class
    {
        /// <summary>
        ///     Contexto de leitura
        /// </summary>
        public TReadContext ReadContext { get; set; }

        /// <summary>
        ///     Contexto de escrita
        /// </summary>
        public TWriteContext WriteContext { get; set; }

        /// <summary>
        ///     Inicializa os contextos
        /// </summary>
        /// <param name="readContext">Contexto somente leitura</param>
        /// <param name="writeContext">Contexto de escrita</param>
        public void Init(TReadContext readContext, TWriteContext writeContext)
        {
            ReadContext = readContext;
            WriteContext = writeContext;
        }

        /// <summary>
        ///     Filtro de paginação
        /// </summary>
        /// <param name="query">IQueryable</param>
        /// <param name="filterPagination">Filtro de paginação</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> Filter(IQueryable<TEntity> query, IFilterPagination filterPagination)
        {
            return query;
        }

        /// <summary>
        ///     Objeto de consulta somente leitura
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetQueryable()
        {
            return ReadContext.GetQueryable<TEntity>();
        }

        /// <summary>
        ///     Objeto de consulta somente leitura
        /// </summary>
        /// <typeparam name="TEntity2">Classe da entidade</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity2> GetQueryable<TEntity2>() where TEntity2 : class
        {
            return ReadContext.Set<TEntity2>().AsNoTracking();
        }

        /// <summary>
        ///     Objeto de escrita
        /// </summary>
        /// <returns></returns>
        public DbSet<TEntity> GetWritable()
        {
            return WriteContext.Set<TEntity>();
        }

        /// <summary>
        ///     Objeto de escrita
        /// </summary>
        /// <typeparam name="TEntity2">Classe da entidade</typeparam>
        /// <returns></returns>
        public DbSet<TEntity2> GetWritable<TEntity2>() where TEntity2 : class
        {
            return WriteContext.Set<TEntity2>();
        }

        /// <summary>
        ///     Validar entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <typeparam name="TEntity">Classe da entidade</typeparam>
        /// <exception cref="RepositoryException"></exception>
        protected virtual void ValidateEntity(TEntity entity)
        {
            if (entity == null)
                throw new RepositoryException((int)EBaseError.ENTITY_NOT_FOUND, BaseErrors.EntityNotFound);
        }

        /// <summary>
        ///     Validar entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <typeparam name="TEntity2">Classe da entidade</typeparam>
        /// <exception cref="RepositoryException"></exception>
        protected virtual void ValidateEntity<TEntity2>(TEntity2 entity) where TEntity2 : class
        {
            if (entity == null)
                throw new RepositoryException((int)EBaseError.ENTITY_NOT_FOUND, BaseErrors.EntityNotFound);
        }

        /// <summary>
        ///     Salva as alterações na conexão de escrita
        /// </summary>
        /// <returns></returns>
        public virtual int SaveChanges()
        {
            return WriteContext.SaveChanges();
        }

        /// <summary>
        ///     Salva as alterações na conexão de escrita (Assíncrono)
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns></returns>
        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await WriteContext.SaveChangesAsync(cancellationToken);
        }
    }
}
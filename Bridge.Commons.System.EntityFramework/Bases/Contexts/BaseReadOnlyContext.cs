using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Commons.System.Enums;
using Bridge.Commons.System.Exceptions;
using Bridge.Commons.System.Resources;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Commons.System.EntityFramework.Bases.Contexts
{
    /// <summary>
    ///     Context base de apenas leitura. (Para context de escrita use BaseWriteContext).
    /// </summary>
    public class BaseReadOnlyContext : DbContext
    {
        /// <summary>
        ///     Construtor
        /// </summary>
        /// <param name="options"></param>
        public BaseReadOnlyContext(DbContextOptions options) : base(options)
        {
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class
        {
            return Set<TEntity>().AsNoTracking();
        }

        /// <summary>
        ///     Salvar alterações
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            throw new RepositoryException((int)EBaseError.INVALID_OPERATION_ON_READONLY_CONNECTION,
                BaseErrors.InvalidOperationOnReadOnlyConnection);
        }

        /// <summary>
        ///     Salvar alterações (assíncrono)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new RepositoryException((int)EBaseError.INVALID_OPERATION_ON_READONLY_CONNECTION,
                BaseErrors.InvalidOperationOnReadOnlyConnection);
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Commons.System.EntityFramework.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Commons.System.EntityFramework.Bases.Contexts
{
    /// <summary>
    ///     Context base de escrita. (Para context de leitura use BaseReadOnlyContext).
    /// </summary>
    public abstract class BaseWriteContext : DbContext
    {
        /// <summary>
        ///     Construtor
        /// </summary>
        /// <param name="options"></param>
        public BaseWriteContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        ///     Salvar alterações
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            GeneratedDate();
            return base.SaveChanges();
        }

        /// <summary>
        ///     Salvar alterações (assíncrono)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            GeneratedDate();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void GeneratedDate()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is IBaseAuditEntity &&
                            (x.State == EntityState.Added || x.State == EntityState.Modified)).ToList();

            if (entities.Count <= 0)
                return;

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow; // current datetime

                if (entity.State == EntityState.Added)
                    ((IBaseAuditEntity)entity.Entity).CreateDate = now;
                else
                    entity.Properties.Where(x => x.Metadata.Name == "CreateDate").First().IsModified = false;

                ((IBaseAuditEntity)entity.Entity).UpdateDate = now;
            }
        }
    }
}
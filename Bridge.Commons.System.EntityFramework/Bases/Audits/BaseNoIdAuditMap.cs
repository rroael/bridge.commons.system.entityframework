using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bridge.Commons.System.EntityFramework.Bases.Audits
{
    public class BaseNoIdAuditMap<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseNoIdAuditEntity
    {
        /// <summary>
        ///     Configura o construtor (builder)
        /// </summary>
        /// <param name="builder"></param>
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.CreateDate)
                .IsRequired();

            builder.Property(e => e.UpdateDate)
                .IsRequired();
        }
    }
}
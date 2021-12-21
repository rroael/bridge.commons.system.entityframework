using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bridge.Commons.System.EntityFramework.Bases.Audits
{
    /// <summary>
    ///     Base Audity Map
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public abstract class BaseAuditMap<TEntity, TId> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseAuditEntity<TId>
        where TId : IConvertible
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
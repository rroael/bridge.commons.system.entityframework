using System;

namespace Bridge.Commons.System.EntityFramework.Contracts
{
    /// <summary>
    ///     Base Audit Entity
    /// </summary>
    public interface IBaseAuditEntity
    {
        /// <summary>
        ///     Criar data
        /// </summary>
        DateTime? CreateDate { get; set; }

        /// <summary>
        ///     Atualizar data
        /// </summary>
        DateTime? UpdateDate { get; set; }
    }
}
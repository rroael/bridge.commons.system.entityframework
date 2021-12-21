using System;
using Bridge.Commons.System.Contracts.Mappers;
using Bridge.Commons.System.EntityFramework.Contracts;
using Bridge.Commons.System.Models;

namespace Bridge.Commons.System.EntityFramework.Bases.Audits
{
    /// <summary>
    ///     Base Audity Entity
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class BaseAuditEntity<TId> : Identifiable<TId>,
        IFromObjectMapper<BaseAudit<TId>, BaseAuditEntity<TId>>, IBaseAuditEntity
        where TId : IConvertible
    {
        #region Implements

        /// <summary>
        ///     Mapear de input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public BaseAuditEntity<TId> MapFrom(BaseAudit<TId> input)
        {
            if (input == null) return this;

            Id = input.Id;
            CreateDate = CreateDate;
            UpdateDate = UpdateDate;

            return this;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Criar data
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        ///     Atualizar data
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        #endregion
    }
}
using Bridge.Commons.System.Contracts.Mappers;
using Bridge.Commons.System.EntityFramework.Contracts;
using Bridge.Commons.System.Models;

namespace Bridge.Commons.System.EntityFramework.Bases.Audits
{
    public abstract class BaseNoIdAuditEntity : BaseNoIdAudit, IFromObjectMapper<BaseNoIdAudit, BaseNoIdAuditEntity>,
        IBaseAuditEntity
    {
        #region Implements

        /// <summary>
        ///     Mapear de input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public BaseNoIdAuditEntity MapFrom(BaseNoIdAudit input)
        {
            if (input == null) return this;

            CreateDate = CreateDate;
            UpdateDate = UpdateDate;

            return this;
        }

        #endregion
    }
}
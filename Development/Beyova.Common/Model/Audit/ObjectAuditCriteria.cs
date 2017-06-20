using System;

namespace Beyova
{
    /// <summary>
    /// Class ObjectAuditCriteria.
    /// </summary>
    public class ObjectAuditCriteria : ObjectAuditCriteria<long>
    {
    }

    /// <summary>
    /// Class ObjectAuditCriteria.
    /// </summary>
    /// <typeparam name="TId">The type of the t identifier.</typeparam>
    public abstract class ObjectAuditCriteria<TId>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public TId Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        /// <value>The name of the entity.</value>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        /// <value>The primary key.</value>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>From stamp.</value>
        public DateTime? FromStamp { get; set; }

        /// <summary>
        /// Gets or sets to stamp.
        /// </summary>
        /// <value>To stamp.</value>
        public DateTime? ToStamp { get; set; }

        /// <summary>
        /// Gets or sets the operated by.
        /// </summary>
        /// <value>The operated by.</value>
        public string OperatedBy { get; set; }

        /// <summary>
        /// Gets or sets the state of the object.
        /// </summary>
        /// <value>The state of the object.</value>
        public ObjectState? ObjectState { get; set; }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>The page number.</value>
        public int? PageNumber { get; set; }
    }
}
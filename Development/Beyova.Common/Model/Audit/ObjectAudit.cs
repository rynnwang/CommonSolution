using System;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class ObjectRawAudit.
    /// </summary>
    public class ObjectRawAudit : ObjectAudit<long, JToken>
    {
    }

    /// <summary>
    /// Class ObjectAudit.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TContent">The type of the content.</typeparam>
    public abstract class ObjectAudit<TId, TContent>
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
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public TContent Content { get; set; }

        /// <summary>
        /// Gets or sets the operated stamp.
        /// </summary>
        /// <value>The operated stamp.</value>
        public DateTime? OperatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the operated by.
        /// </summary>
        /// <value>The operated by.</value>
        public string OperatedBy { get; set; }

        /// <summary>
        /// Gets or sets the state of the object.
        /// </summary>
        /// <value>The state of the object.</value>
        public ObjectState ObjectState { get; set; }
    }
}
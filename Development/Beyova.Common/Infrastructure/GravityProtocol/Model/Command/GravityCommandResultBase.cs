using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityCommandResultBase.
    /// </summary>
    public abstract class GravityCommandResultBase : IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the request key.
        /// </summary>
        /// <value>The request key.</value>
        public Guid? RequestKey { get; set; }

        /// <summary>
        /// Gets or sets the client key.
        /// </summary>
        /// <value>The client key.</value>
        public Guid? ClientKey { get; set; }
    }
}
using System;

namespace Beyova
{
    /// <summary>
    /// Class AdminSessionBase.
    /// </summary>
    public class AdminSessionBase : IOwnerIdentifiable, IProductIdentifier
    {
        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        public Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the product key.
        /// </summary>
        /// <value>The product key.</value>
        public Guid? ProductKey { get; set; }
    }
}
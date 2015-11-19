using System;

namespace ifunction.Model
{
    /// <summary>
    /// Class AdminSessionBase.
    /// </summary>
    public class AdminSessionBase
    {
        /// <summary>
        /// Gets or sets the domain key.
        /// </summary>
        /// <value>
        /// The domain key.
        /// </value>
        public Guid? DomainKey { get; set; }

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
    }
}

using System;

namespace Beyova
{
    /// <summary>
    /// Class AccountSecurityUpdateToken.
    /// </summary>
    public class AccountSecurityUpdateToken : IExpirable
    {
        /// <summary>
        /// Gets or sets the access identifier.
        /// </summary>
        /// <value>The access identifier.</value>
        public string AccessIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the security token.
        /// </summary>
        /// <value>The security token.</value>
        public string SecurityToken { get; set; }

        /// <summary>
        /// Gets or sets the update token.
        /// </summary>
        /// <value>The update token.</value>
        public string UpdateToken { get; set; }

        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>The expired stamp.</value>
        public DateTime? ExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>The user key.</value>
        public Guid? UserKey { get; set; }
    }
}
using System;

namespace Beyova
{
    /// <summary>
    /// Class SSOAuthorizationPartnerCriteria.
    /// </summary>
    public class SSOAuthorizationPartnerCriteria
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        public Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the callback URL.
        /// </summary>
        /// <value>The callback URL.</value>
        public string CallbackUrl { get; set; }
    }
}

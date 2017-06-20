using System;

namespace Beyova
{
    /// <summary>
    /// Class SSOAuthorizationPartner.
    /// </summary>
    public class SSOAuthorizationPartner : BaseObject, IOwnerIdentifiable
    {
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
        /// Gets or sets the token expiration. (Minute)
        /// </summary>
        /// <value>The token expiration.</value>
        public int? TokenExpiration { get; set; }

        /// <summary>
        /// Gets or sets the callback URL.
        /// </summary>
        /// <value>The callback URL.</value>
        public string CallbackUrl { get; set; }
    }
}
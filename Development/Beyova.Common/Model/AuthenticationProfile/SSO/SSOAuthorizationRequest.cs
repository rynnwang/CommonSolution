using System;

namespace Beyova
{
    /// <summary>
    /// Class SSOAuthorizationRequest. 
    /// For Token Exchange, use <c>ClientRequestId</c>, <c>PartnerKey</c> and <c>UserKey</c>.
    /// For SSO, use <c>ClientRequestId</c>, <c>PartnerKey</c> and <c>CallbackUrl</c>.
    /// </summary>
    public class SSOAuthorizationRequest
    {
        /// <summary>
        /// Gets or sets the client request identifier.
        /// </summary>
        /// <value>The client request identifier.</value>
        public string ClientRequestId { get; set; }

        /// <summary>
        /// Gets or sets the provider key.
        /// </summary>
        /// <value>The provider key.</value>
        public Guid? PartnerKey { get; set; }

        /// <summary>
        /// Gets or sets the callback URL.
        /// </summary>
        /// <value>The callback URL.</value>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>The user key.</value>
        public Guid? UserKey { get; set; }
    }
}

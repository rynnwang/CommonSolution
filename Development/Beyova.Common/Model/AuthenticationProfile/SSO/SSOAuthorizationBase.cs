namespace Beyova
{
    /// <summary>
    /// Class SSOAuthorizationBase.
    /// </summary>
    public class SSOAuthorizationBase
    {
        /// <summary>
        /// Gets or sets the client request identifier.
        /// </summary>
        /// <value>The client request identifier.</value>
        public string ClientRequestId { get; set; }

        /// <summary>
        /// Gets or sets the authorization token.
        /// </summary>
        /// <value>The authorization token.</value>
        public string AuthorizationToken { get; set; }
    }
}
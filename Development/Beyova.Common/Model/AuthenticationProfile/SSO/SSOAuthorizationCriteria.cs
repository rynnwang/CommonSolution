using System;

namespace Beyova
{
    /// <summary>
    /// Class SSOAuthorizationCriteria.
    /// </summary>
    public class SSOAuthorizationCriteria : BasePageIndexedCriteria
    {
        /// <summary>
        /// Gets or sets the partner key.
        /// </summary>
        /// <value>The partner key.</value>
        public Guid? PartnerKey { get; set; }

        /// <summary>
        /// Gets or sets the authorization token.
        /// </summary>
        /// <value>The authorization token.</value>
        public string AuthorizationToken { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is used.
        /// </summary>
        /// <value><c>null</c> if [is used] contains no value, <c>true</c> if [is used]; otherwise, <c>false</c>.</value>
        public bool? IsUsed { get; set; }

        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>The user key.</value>
        public Guid? UserKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SSOAuthorizationCriteria"/> class.
        /// </summary>
        public SSOAuthorizationCriteria() : base()
        {

        }
    }
}

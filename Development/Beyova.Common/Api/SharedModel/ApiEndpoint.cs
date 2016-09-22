using System;

namespace Beyova.Api
{
    /// <summary>
    /// Class ApiEndpoint.
    /// </summary>
    public class ApiEndpoint : UriEndpoint
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>The account.</value>
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the secondary token.
        /// </summary>
        /// <value>The secondary token.</value>
        public string SecondaryToken { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}api/{1}/", base.ToString(), Version.SafeToString("v1"));
        }

        /// <summary>
        /// To the URI.
        /// </summary>
        /// <returns></returns>
        public override Uri ToUri()
        {
            return new Uri(this.ToString());
        }

        /// <summary>
        /// To the URI.
        /// </summary>
        /// <param name="appendApiSuffix">if set to <c>true</c> [append API suffix].</param>
        /// <returns></returns>
        public Uri ToUri(bool appendApiSuffix)
        {
            return appendApiSuffix ? ToUri() : new Uri(base.ToString());
        }
    }
}

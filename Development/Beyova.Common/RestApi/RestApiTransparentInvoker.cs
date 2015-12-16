using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class RestApiTransparentInvoker.
    /// </summary>
    public class RestApiTransparentInvoker
    {
        /// <summary>
        /// Gets or sets the destination base URL.
        /// </summary>
        /// <value>The destination base URL.</value>
        public string DestinationUrl { get; protected set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>The HTTP method.</value>
        public string HttpMethod { get; protected set; }

        /// <summary>
        /// Gets or sets the authentication header.
        /// </summary>
        /// <value>The authentication header.</value>
        public string AuthenticationHeader { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiTransparentInvoker" /> class.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="destinationUrl">The destination URL.</param>
        /// <param name="authenticationHeader">The authentication header.</param>
        protected RestApiTransparentInvoker(string httpMethod, string destinationUrl, string authenticationHeader = HttpConstants.HttpHeader.TOKEN)
        {
            this.HttpMethod = httpMethod;
            this.DestinationUrl = destinationUrl;
            this.AuthenticationHeader = authenticationHeader;
        }
    }
}
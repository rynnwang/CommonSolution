using System;

namespace Beyova
{
    /// <summary>
    /// Class UriEndpoint.
    /// </summary>
    public class UriEndpoint
    {
        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the path or path prefix. Example: "/path", "/path1/path2"
        /// </summary>
        /// <value>
        /// The path prefix.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int? Port { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance. Format: {Protocol}://{Host}:{Port?}{PathPrefix}/
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            var pathPrefix = Path.SafeToString().Trim('/');
            pathPrefix = string.IsNullOrWhiteSpace(pathPrefix) ? string.Empty : ("/" + pathPrefix);

            return this.Port.HasValue ?
                string.Format("{0}://{1}:{2}{3}/", Protocol.SafeToString(HttpConstants.HttpProtocols.Http), Host.SafeToString(HttpConstants.HttpValues.Localhost), Port.Value, pathPrefix) :
                string.Format("{0}://{1}{2}/", Protocol.SafeToString(HttpConstants.HttpProtocols.Http), Host.SafeToString(HttpConstants.HttpValues.Localhost), pathPrefix);
        }

        /// <summary>
        /// To the URI.
        /// </summary>
        /// <returns></returns>
        public virtual Uri ToUri()
        {
            return new Uri(this.ToString());
        }

        /// <summary>
        /// Fills the endpoint by URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="endpoint">The endpoint.</param>
        protected static void FillEndpointByUri(Uri uri, UriEndpoint endpoint)
        {
            if (uri != null && endpoint != null)
            {
                endpoint.Protocol = uri.Scheme;
                endpoint.Host = uri.Host;
                endpoint.Port = uri.IsDefaultPort ? null : uri.Port as int?;
                endpoint.Path = "/" + uri.AbsolutePath.TrimStart('/').SubStringBeforeFirstMatch('/');
            }
        }
    }
}

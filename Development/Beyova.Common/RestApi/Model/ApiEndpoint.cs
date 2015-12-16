namespace Beyova.RestApi
{
    /// <summary>
    /// Class ApiEndpoint.
    /// </summary>
    public class ApiEndpoint
    {
        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        public string Protocol { get; set; }

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
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}://{1}:{2}/api/{3}/", Protocol.SafeToString("http"), Host.SafeToString("localhost"), Port ?? 80, Version.SafeToString("v1"));
        }
    }
}

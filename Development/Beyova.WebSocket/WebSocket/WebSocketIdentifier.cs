using System;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace ifunction.WebSocket
{
    /// <summary>
    /// Class WebSocketIdentifier.
    /// </summary>
    public class WebSocketIdentifier
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>The device identifier.</value>
        public string DeviceIdentifier { get; set; }
    }
}

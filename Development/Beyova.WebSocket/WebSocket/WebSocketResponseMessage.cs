using System;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Beyova.WebSocketComponent
{
    /// <summary>
    /// Class WebSocketResponseMessage.
    /// </summary>
    public class WebSocketResponseMessage
    {
        /// <summary>
        /// Gets or sets the message meta.
        /// </summary>
        /// <value>The message meta.</value>
        public WebSocketMessageMeta MessageMeta { get; set; }

        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        /// <value>The destination.</value>
        public WebSocketIdentifier Destination { get; set; }
    }
}

using System;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Beyova.WebSocketComponent
{
    /// <summary>
    /// Class WebSocketMessageMeta.
    /// </summary>
    public class WebSocketMessageMeta
    {
        /// <summary>
        /// Gets or sets the name of the operation.
        /// </summary>
        /// <value>The name of the operation.</value>
        public string OperationName { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public string Body { get; set; }
    }
}

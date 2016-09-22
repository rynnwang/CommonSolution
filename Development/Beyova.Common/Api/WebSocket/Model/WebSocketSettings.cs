using System.Collections.Generic;
using System.Reflection;
using Beyova.Api;
using Newtonsoft.Json;

namespace Beyova.Api.WebSockets
{
    /// <summary>
    /// Class WebSocketSettings.
    /// </summary>
    public class WebSocketSettings : ApiSettings
    {
        #region Message Alias

        /// <summary>
        /// Gets or sets the message action alias.
        /// </summary>
        /// <value>The message action alias.</value>
        public string MessageActionAlias { get; set; }

        /// <summary>
        /// Gets or sets the message body alias.
        /// </summary>
        /// <value>The message body alias.</value>
        public string MessageBodyAlias { get; set; }

        /// <summary>
        /// Gets or sets the message exception alias.
        /// </summary>
        /// <value>The message exception alias.</value>
        public string MessageExceptionAlias { get; set; }

        /// <summary>
        /// Gets or sets the message referrer alias.
        /// </summary>
        /// <value>The message referrer alias.</value>
        public string MessageReferrerAlias { get; set; }

        #endregion
    }
}

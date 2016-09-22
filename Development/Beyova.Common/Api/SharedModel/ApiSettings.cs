using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace Beyova.Api
{
    /// <summary>
    /// Class ApiSettings.
    /// </summary>
    public abstract class ApiSettings
    {
        /// <summary>
        /// Gets or sets the name. Name would not involve API feature, but would help <see cref="ContextHelper"/> find token delegate for authentication.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the API tracking.
        /// </summary>
        /// <value>The API tracking.</value>
        public IApiTracking ApiTracking { get; set; }

        /// <summary>
        /// Gets or sets the token header key.
        /// </summary>
        /// <value>The token header key.</value>
        public string TokenHeaderKey { get; set; }

        /// <summary>
        /// Gets or sets the client identifier header key.
        /// </summary>
        /// <value>The client identifier header key.</value>
        public string ClientIdentifierHeaderKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [tracking event].
        /// </summary>
        /// <value><c>true</c> if [tracking event]; otherwise, <c>false</c>.</value>
        public bool TrackingEvent { get; set; }
    }
}

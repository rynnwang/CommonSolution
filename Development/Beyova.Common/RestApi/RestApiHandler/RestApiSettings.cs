using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class RestApiSettings.
    /// </summary>
    public class RestApiSettings
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

        /// <summary>
        /// Gets or sets a value indicating whether [enable content compression].
        /// </summary>
        /// <value><c>true</c> if [enable content compression]; otherwise, <c>false</c>.</value>
        public bool EnableContentCompression { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable output full exception information].
        /// </summary>
        /// <value><c>true</c> if [enable output full exception information]; otherwise, <c>false</c>.</value>
        public bool EnableOutputFullExceptionInfo { get; set; }

        /// <summary>
        /// Gets or sets the json converters.
        /// </summary>
        /// <value>The json converters.</value>
        public JsonConverter[] JsonConverters { get; set; }

        /// <summary>
        /// Gets or sets the event handlers.
        /// </summary>
        /// <value>The event handlers.</value>
        public RestApiEventHandlers EventHandlers { get; set; }
    }
}

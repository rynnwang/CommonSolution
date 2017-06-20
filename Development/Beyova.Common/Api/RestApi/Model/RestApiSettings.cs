using Beyova.Api;
using Newtonsoft.Json;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class RestApiSettings.
    /// </summary>
    public class RestApiSettings : ApiSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether [enable content compression].
        /// </summary>
        /// <value><c>true</c> if [enable content compression]; otherwise, <c>false</c>.</value>
        public bool EnableContentCompression { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether [omit exception detail].
        /// </summary>
        /// <value><c>true</c> if [omit exception detail]; otherwise, <c>false</c>.</value>
        public bool OmitExceptionDetail { get; set; }

        /// <summary>
        /// Gets or sets the original ip address header key. If not specified, use <see cref="HttpConstants.HttpHeader.ORIGINAL"/>.
        /// </summary>
        /// <value>The original ip address header key.</value>
        public string OriginalIpAddressHeaderKey { get; set; }

        /// <summary>
        /// Gets or sets the original user agent header key.
        /// </summary>
        /// <value>The original user agent header key.</value>
        public string OriginalUserAgentHeaderKey { get; set; }

        /// <summary>
        /// Gets or sets the cache header key. If specific value is later than returned object's stamp, return 304. It is effected when return type is <see cref="ISimpleBaseObject"/>
        /// </summary>
        /// <value>The cache header key.</value>
        public string CacheHeaderKey { get; set; }

        /// <summary>
        /// Gets or sets the version match key. If specific value is equals snapshot key,return 304. It is effected when return type is <see cref="ISnapshotable"/>
        /// </summary>
        /// <value>The version match key.</value>
        public string VersionMatchKey { get; set; }
    }
}
using System;
using Newtonsoft.Json;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ApiEventLogBase.
    /// </summary>
    public class ApiEventLogBase : ApiLogBase
    {
        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>The name of the resource.</value>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the resource entity key.
        /// </summary>
        /// <value>The resource entity key.</value>
        public string ResourceEntityKey { get; set; }

        /// <summary>
        /// Gets or sets the full name of the API.
        /// </summary>
        /// <value>The full name of the API.</value>
        public string ApiFullName { get; set; }

        /// <summary>
        /// Gets or sets the exception key.
        /// </summary>
        /// <value>The exception key.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ExceptionKey { get; set; }

        /// <summary>
        /// Gets or sets the culture code.
        /// </summary>
        /// <value>The culture code.</value>
        public string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// Commonly, it can be device id, PC name, etc.
        /// </summary>
        /// <value>The client identifier.</value>
        public string ClientIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the raw URL.
        /// </summary>
        /// <value>The raw URL.</value>
        public string RawUrl { get; set; }

        /// <summary>
        /// Gets or sets the referrer URL.
        /// </summary>
        /// <value>The referrer URL.</value>
        public string ReferrerUrl { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>The platform.</value>
        public PlatformType? Platform { get; set; }

        /// <summary>
        /// Gets or sets the type of the device.
        /// </summary>
        /// <value>The type of the device.</value>
        public DeviceType? DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the trace identifier.
        /// </summary>
        /// <value>The trace identifier.</value>
        public string TraceId { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the operator credential.
        /// </summary>
        /// <value>The operator credential.</value>
        public BaseCredential OperatorCredential { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [hit API cache].
        /// </summary>
        /// <value><c>true</c> if [hit API cache]; otherwise, <c>false</c>.</value>
        public bool HitApiCache { get; set; }

        /// <summary>
        /// Gets or sets the referrer.
        /// </summary>
        /// <value>The referrer.</value>
        public string Referrer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventLogBase"/> class.
        /// </summary>
        public ApiEventLogBase()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventLogBase"/> class.
        /// </summary>
        /// <param name="eventLogBase">The event log base.</param>
        public ApiEventLogBase(ApiEventLogBase eventLogBase)
            : base(eventLogBase)
        {
            if (eventLogBase != null)
            {
                this.ExceptionKey = eventLogBase.ExceptionKey;
                this.ResourceEntityKey = eventLogBase.ResourceEntityKey;
                this.CultureCode = eventLogBase.CultureCode;
                this.ClientIdentifier = eventLogBase.ClientIdentifier;
                this.ApiFullName = eventLogBase.ApiFullName;
                this.IpAddress = eventLogBase.IpAddress;
                this.RawUrl = eventLogBase.RawUrl;
                this.UserAgent = eventLogBase.UserAgent;
                this.ResourceName = eventLogBase.ResourceName;
                this.ExceptionKey = eventLogBase.ExceptionKey;
                this.Platform = eventLogBase.Platform;
                this.DeviceType = eventLogBase.DeviceType;
                this.OperatorCredential = eventLogBase.OperatorCredential;
            }
        }
    }
}
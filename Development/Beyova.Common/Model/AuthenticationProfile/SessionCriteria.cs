using System;
using Beyova;

namespace Beyova
{
    /// <summary>
    /// Class SessionCriteria.
    /// </summary>
    public class SessionCriteria : BasePageIndexedCriteria
    {
        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>The user key.</value>
        public Guid? UserKey { get; set; }

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
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expired.
        /// </summary>
        /// <value><c>null</c> if [is expired] contains no value, <c>true</c> if [is expired]; otherwise, <c>false</c>.</value>
        public bool? IsExpired { get; set; }

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
    }
}

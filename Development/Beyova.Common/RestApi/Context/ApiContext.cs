using System.Collections.Generic;
using Beyova;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class ApiContext.
    /// </summary>
    public class ApiContext
    {
        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>The platform.</value>
        public PlatformType Platform { get; set; }

        /// <summary>
        /// Gets or sets the type of the device.
        /// </summary>
        /// <value>The type of the device.</value>
        public DeviceType DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the current credential.
        /// </summary>
        /// <value>The current credential.</value>
        public ICredential CurrentCredential { get; set; }

        /// <summary>
        /// Gets the current permission identifiers.
        /// </summary>
        /// <value>The current permission identifiers.</value>
        public IPermissionIdentifiers CurrentPermissionIdentifiers
        {
            get { return CurrentCredential as IPermissionIdentifiers; }
        }

        /// <summary>
        /// Gets or sets the current user information.
        /// </summary>
        /// <value>The current user information.</value>
        public IUserInfo CurrentUserInfo
        {
            get { return CurrentCredential as IUserInfo; }
            set { CurrentCredential = value; }
        }

        /// <summary>
        /// Gets or sets the permission identifiers.
        /// </summary>
        /// <value>The permission identifiers.</value>
        public List<string> PermissionIdentifiers { get; protected set; }

        /// <summary>
        /// Gets or sets the culture code.
        /// </summary>
        /// <value>The culture code.</value>
        public string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets the additional headers.
        /// </summary>
        /// <value>The additional headers.</value>
        public Dictionary<string, string> AdditionalHeaders { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext"/> class.
        /// </summary>
        public ApiContext()
        {
            PermissionIdentifiers = new List<string>();
            AdditionalHeaders = new Dictionary<string, string>();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return CurrentCredential?.Name.SafeToString();
        }
    }
}

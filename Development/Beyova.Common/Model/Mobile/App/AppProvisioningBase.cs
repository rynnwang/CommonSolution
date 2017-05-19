using System;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class AppProvisioningBase.
    /// </summary>
    public class AppProvisioningBase : IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the platform key.
        /// </summary>
        /// <value>
        /// The platform key.
        /// </value>
        public Guid? PlatformKey { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public JObject Content { get; set; }
    }
}

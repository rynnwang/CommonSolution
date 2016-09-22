using System;
using Beyova;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class RemoteConfigurationObject.
    /// </summary>
    public class RemoteConfigurationObject : IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the snapshot key.
        /// </summary>
        /// <value>The snapshot key.</value>
        public Guid? SnapshotKey { get; set; }

        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        public Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public JToken Configuration { get; set; }
    }
}

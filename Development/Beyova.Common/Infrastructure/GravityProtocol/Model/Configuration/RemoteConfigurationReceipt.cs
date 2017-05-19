using System;
using Beyova;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class RemoteConfigurationReceipt.
    /// </summary>
    public class RemoteConfigurationReceipt
    {
        /// <summary>
        /// Gets or sets the stamp.
        /// </summary>
        /// <value>The stamp.</value>
        public DateTime? Stamp { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the snapshot key.
        /// </summary>
        /// <value>The snapshot key.</value>
        public Guid? SnapshotKey { get; set; }
    }
}

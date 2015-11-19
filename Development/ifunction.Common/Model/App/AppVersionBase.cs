using System;

namespace ifunction.Model
{
    /// <summary>
    /// Class AppVersionBase.
    /// </summary>
    public class AppVersionBase
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the platform key.
        /// </summary>
        /// <value>
        /// The platform key.
        /// </value>
        public Guid? PlatformKey { get; set; }

        /// <summary>
        /// Gets or sets the latest build.
        /// </summary>
        /// <value>
        /// The latest build.
        /// </value>
        public int LatestBuild { get; set; }

        /// <summary>
        /// Gets or sets the latest version.
        /// </summary>
        /// <value>
        /// The latest version.
        /// </value>
        public string LatestVersion { get; set; }

        /// <summary>
        /// Gets or sets the minimum required build.
        /// </summary>
        /// <value>
        /// The minimum required build.
        /// </value>
        public int MinRequiredBuild { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>
        /// The note.
        /// </value>
        public string Note { get; set; }
    }
}

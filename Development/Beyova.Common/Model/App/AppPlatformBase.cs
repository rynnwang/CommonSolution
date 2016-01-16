using System;

namespace Beyova
{
    /// <summary>
    /// Class AppPlatformBase.
    /// </summary>
    public class AppPlatformBase
    {
        /// <summary>
        /// Gets or sets the type of the platform.
        /// </summary>
        /// <value>
        /// The type of the platform.
        /// </value>
        public PlatformType PlatformType { get; set; }

        /// <summary>
        /// Gets or sets the application URL.
        /// </summary>
        /// <value>The application URL.</value>
        public string AppStoreUrl { get; set; }

        /// <summary>
        /// Gets or sets the bundle identifier.
        /// </summary>
        /// <value>
        /// The bundle identifier.
        /// </value>
        public string BundleId { get; set; }
    }
}

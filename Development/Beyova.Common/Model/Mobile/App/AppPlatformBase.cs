using System;

namespace Beyova
{
    /// <summary>
    /// Class AppPlatformBase.
    /// </summary>
    public class AppPlatformBase : IAppPlatform
    {
        /// <summary>
        /// Gets or sets the type of the platform.
        /// </summary>
        /// <value>
        /// The type of the platform.
        /// </value>
        public PlatformType PlatformType { get; set; }

        /// <summary>
        /// Gets or sets the minimum os version.
        /// </summary>
        /// <value>The minimum os version.</value>
        public string MinOSVersion { get; set; }

        /// <summary>
        /// Gets or sets the application URL.
        /// </summary>
        /// <value>The application URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the bundle identifier.
        /// </summary>
        /// <value>
        /// The bundle identifier.
        /// </value>
        public string BundleId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}

namespace Beyova
{
    /// <summary>
    /// Interface IAppPlatform
    /// </summary>
    public interface IAppPlatform
    {
        /// <summary>
        /// Gets or sets the type of the platform.
        /// </summary>
        /// <value>
        /// The type of the platform.
        /// </value>
        PlatformType PlatformType { get; set; }

        /// <summary>
        /// Gets or sets the minimum os version.
        /// </summary>
        /// <value>The minimum os version.</value>
        string MinOSVersion { get; set; }

        /// <summary>
        /// Gets or sets the application URL.
        /// </summary>
        /// <value>The application URL.</value>
        string Url { get; set; }

        /// <summary>
        /// Gets or sets the bundle identifier.
        /// </summary>
        /// <value>
        /// The bundle identifier.
        /// </value>
        string BundleId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }
    }
}
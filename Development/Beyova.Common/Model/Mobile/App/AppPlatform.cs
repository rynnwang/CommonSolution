using System;

namespace Beyova
{
    /// <summary>
    /// Class AppPlatform.
    /// </summary>
    public class AppPlatform : AppPlatformBase, IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
    }
}
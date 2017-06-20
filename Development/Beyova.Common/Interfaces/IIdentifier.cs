using System;

namespace Beyova
{
    /// <summary>
    /// Interface for object IIdentifier.
    /// </summary>
    public interface IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        Guid? Key { get; set; }
    }
}
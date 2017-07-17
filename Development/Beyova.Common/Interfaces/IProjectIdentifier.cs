using System;

namespace Beyova
{
    /// <summary>
    /// Interface IProjectIdentifier
    /// </summary>
    public interface IProjectIdentifier
    {
        /// <summary>
        /// Gets or sets the project key.
        /// </summary>
        /// <value>
        /// The project key.
        /// </value>
        Guid? ProjectKey { get; set; }
    }
}
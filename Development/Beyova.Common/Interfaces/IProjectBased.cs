using System;

namespace Beyova
{
    /// <summary>
    /// Interface IProjectBased
    /// </summary>
    public interface IProjectBased
    {
        /// <summary>
        /// Gets or sets the project key.
        /// </summary>
        /// <value>The project key.</value>
        Guid? ProjectKey { get; set; }
    }
}
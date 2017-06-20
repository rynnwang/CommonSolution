using System;

namespace Beyova
{
    /// <summary>
    /// Interface IOwnerIdentifiable
    /// </summary>
    public interface IOwnerIdentifiable
    {
        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        Guid? OwnerKey { get; set; }
    }
}
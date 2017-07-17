using System;

namespace Beyova
{
    /// <summary>
    /// Interface IProductIdentifier
    /// </summary>
    public interface IProductIdentifier
    {
        /// <summary>
        /// Gets or sets the product key.
        /// </summary>
        /// <value>The product key.</value>
        Guid? ProductKey { get; set; }
    }
}
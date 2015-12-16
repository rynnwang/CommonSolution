using System;

namespace Beyova.Model
{
    /// <summary>
    /// Interface IProvisioningItem
    /// </summary>
    /// <typeparam name="TApplication">The type of the application.</typeparam>
    public interface IProvisioningItem<TApplication>
        where TApplication : struct, IConvertible
    {
        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>The application.</value>
        TApplication Application { get; set; }

        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        /// <value>The module.</value>
        string Module { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        string Value { get; set; }
    }
}

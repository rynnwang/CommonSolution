using System;

namespace Beyova
{
    /// <summary>
    /// Interface IProvisioningCriteria
    /// </summary>
    /// <typeparam name="TApplication">The type of the t application.</typeparam>
    public interface IProvisioningCriteria<TApplication> : IIdentifier
        where TApplication : struct, IConvertible
    {
        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>The application.</value>
        TApplication? Application { get; set; }

        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets the module. Optional.
        /// </summary>
        /// <value>The module.</value>
        string Module { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
    }
}

using System;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Interface IProvisioningObject
    /// </summary>
    /// <typeparam name="TApplication">The type of the application.</typeparam>
    public interface IProvisioningObject<TApplication> : IBaseObject
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
        /// Gets or sets the module.
        /// </summary>
        /// <value>The module.</value>
        string Module { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        JToken Value { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Beyova;
using Beyova.RestApi;

namespace Beyova.CommonServiceInterface
{
    /// <summary>
    /// Interface IProvisioningService
    /// </summary>
    /// <typeparam name="TProvisioning">The type of the t provisioning.</typeparam>
    /// <typeparam name="TProvisioningCriteria">The type of the t provisioning criteria.</typeparam>
    /// <typeparam name="TApplication">The type of the t application.</typeparam>
    public interface IProvisioningService<TProvisioning, TProvisioningCriteria, TApplication>
        where TProvisioning : IProvisioningItem<TApplication>
        where TProvisioningCriteria : IProvisioningCriteria<TApplication>
        where TApplication : struct, IConvertible
    {
        /// <summary>
        /// Saves the provisioning item.
        /// </summary>
        /// <param name="provisioningItem">The provisioning item.</param>
        [ApiOperation("Provisioning", HttpConstants.HttpMethod.Put)]
        void SaveProvisioningItem(TProvisioning provisioningItem);

        /// <summary>
        /// Queries the provisioning item.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TProvisioning&gt;.</returns>
        [ApiOperation("Provisioning", HttpConstants.HttpMethod.Post)]
        List<TProvisioning> QueryProvisioningItem(TProvisioningCriteria criteria);
    }
}

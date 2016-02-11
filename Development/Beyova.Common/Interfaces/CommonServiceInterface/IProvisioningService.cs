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
    [TokenRequired]
    public interface IProvisioningService<TProvisioning, TProvisioningCriteria, TApplication>
          where TProvisioning : IProvisioningObject<TApplication>
          where TProvisioningCriteria : IProvisioningCriteria<TApplication>
          where TApplication : struct, IConvertible
    {
        /// <summary>
        /// Saves the user provisioning object.
        /// </summary>
        /// <param name="provisioningObject">The provisioning item object.</param>
        [ApiOperation("ProvisioningObject", HttpConstants.HttpMethod.Put, "User")]
        void SaveUserProvisioningObject(TProvisioning provisioningObject);

        /// <summary>
        /// Saves the global provisioning object.
        /// </summary>
        /// <param name="provisioningObject">The provisioning object.</param>
        [ApiOperation("ProvisioningObject", HttpConstants.HttpMethod.Put, "Global")]
        [ApiPermission(CommonServiceConstants.Permission.ProvisioningAdministration, ApiPermission.Required)]
        void SaveGlobalProvisioningObject(TProvisioning provisioningObject);

        /// <summary>
        /// Queries the user provisioning object.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TProvisioning&gt;.</returns>
        [ApiOperation("ProvisioningObject", HttpConstants.HttpMethod.Post)]
        List<TProvisioning> QueryUserProvisioningObject(TProvisioningCriteria criteria);

        /// <summary>
        /// Queries the global provisioning object.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TProvisioning&gt;.</returns>
        [ApiOperation("ProvisioningObject", HttpConstants.HttpMethod.Post, "Global")]
        [ApiPermission(CommonServiceConstants.Permission.ProvisioningAdministration, ApiPermission.Required)]
        List<TProvisioning> QueryGlobalProvisioningObject(TProvisioningCriteria criteria);
    }
}

using System;
using System.Collections.Generic;
using Beyova.Api;
using Beyova.RestApi;

namespace Beyova.CommonServiceInterface
{
    /// <summary>
    /// Interface IProvisioningService
    /// </summary>
    [TokenRequired]
    public interface IProvisioningService<TAppProvisioning>
         where TAppProvisioning : AppProvisioningBase
    {
        #region Service Configuration management

        /// <summary>
        /// Creates the or update configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.Configuration, HttpConstants.HttpMethod.Put)]
        [ApiPermission(CommonServiceConstants.Permission.RemoteConfigurationAdministration)]
        Guid? CreateOrUpdateConfiguration(RemoteConfigurationObject configuration);

        /// <summary>
        /// Queries the configuration.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;RemoteConfigurationObject&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.Configuration, HttpConstants.HttpMethod.Post)]
        [ApiPermission(CommonServiceConstants.Permission.RemoteConfigurationAdministration)]
        List<RemoteConfigurationObject> QueryConfiguration(RemoteConfigurationCriteria criteria);

        /// <summary>
        /// Queries the configuration snapshot.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;RemoteConfigurationObject&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.Configuration, HttpConstants.HttpMethod.Post, CommonServiceConstants.ActionName.Snapshot)]
        [ApiPermission(CommonServiceConstants.Permission.RemoteConfigurationAdministration)]
        List<RemoteConfigurationObject> QueryConfigurationSnapshot(RemoteConfigurationCriteria criteria);

        #endregion

        #region App Provisioning

        /// <summary>
        /// Queries the application provisioning.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AppProvisioning, HttpConstants.HttpMethod.Post)]
        [ApiPermission(CommonServiceConstants.Permission.AppProvisioningAdministration)]
        List<TAppProvisioning> QueryAppProvisioning(AppProvisioningCriteria criteria);

        /// <summary>
        /// Queries the application provisioning snapshot.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AppProvisioning, HttpConstants.HttpMethod.Post, CommonServiceConstants.ResourceName.Snapshot)]
        [ApiPermission(CommonServiceConstants.Permission.AppProvisioningAdministration)]
        List<TAppProvisioning> QueryAppProvisioningSnapshot(AppProvisioningCriteria criteria);

        /// <summary>
        /// Creates the or update application provisioning.
        /// </summary>
        /// <param name="appProvisioning">The application provisioning.</param>
        /// <returns></returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AppProvisioning, HttpConstants.HttpMethod.Put)]
        [ApiPermission(CommonServiceConstants.Permission.AppProvisioningAdministration)]
        Guid? CreateOrUpdateAppProvisioning(TAppProvisioning appProvisioning);

        /// <summary>
        /// Deletes the application provisioning.
        /// </summary>
        /// <param name="key">The key.</param>
        [ApiOperation(CommonServiceConstants.ResourceName.AppProvisioning, HttpConstants.HttpMethod.Delete)]
        [ApiPermission(CommonServiceConstants.Permission.AppProvisioningAdministration)]
        void DeleteAppProvisioning(Guid? key);

        /// <summary>
        /// Gets the application provisioning.
        /// </summary>
        /// <param name="platformKey">The platform key.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AppProvisioning, HttpConstants.HttpMethod.Get)]
        [ApiPermission(CommonServiceConstants.Permission.AppProvisioningAdministration)]
        [TokenRequired(false)]
        TAppProvisioning GetAppProvisioning(Guid? platformKey, string name = null);

        #endregion
    }
}

using System;
using System.Collections.Generic;
using Beyova.RestApi;

namespace Beyova.CommonServiceInterface
{
    /// <summary>
    /// Interface IProvisioningService
    /// </summary>
    public interface IProvisioningService
    {
        #region Configuration management

        /// <summary>
        /// Creates the or update configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation("Configuration", HttpConstants.HttpMethod.Put)]
        Guid? CreateOrUpdateConfiguration(RemoteConfigurationObject configuration);

        /// <summary>
        /// Queries the configuration.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;RemoteConfigurationObject&gt;.</returns>
        [ApiOperation("Configuration", HttpConstants.HttpMethod.Post)]
        List<RemoteConfigurationObject> QueryConfiguration(RemoteConfigurationCriteria criteria);

        /// <summary>
        /// Queries the configuration snapshot.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;RemoteConfigurationObject&gt;.</returns>
        [ApiOperation("Configuration", HttpConstants.HttpMethod.Post, "Snapshot")]
        List<RemoteConfigurationObject> QueryConfigurationSnapshot(RemoteConfigurationCriteria criteria);

        #endregion   
    }
}

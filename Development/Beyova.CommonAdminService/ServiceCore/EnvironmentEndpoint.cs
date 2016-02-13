using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.CommonAdminService.DataAccessController;

namespace Beyova.CommonAdminService
{
    partial class CommonAdminService
    {
        /// <summary>
        /// Creates the or update environment endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CreateOrUpdateEnvironmentEndpoint(EnvironmentEndpoint endpoint)
        {
            try
            {
                endpoint.CheckNullObject("endpoint");

                using (var controller = new EnvironmentEndpointDataAccessController())
                {
                    return controller.CreateOrUpdateEnvironmentEndpoint(endpoint, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("CreateOrUpdateEnvironmentEndpoint", endpoint);
            }
        }

        /// <summary>
        /// Queries the environment endpoint.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="code">The code.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>List&lt;EnvironmentEndpoint&gt;.</returns>
        public List<EnvironmentEndpoint> QueryEnvironmentEndpoint(Guid? key = null, string code = null, string environment = null)
        {
            try
            {
                using (var controller = new EnvironmentEndpointDataAccessController())
                {
                    return controller.QueryEnvironmentEndpoint(key, code, environment);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryEnvironmentEndpoint", new { key, code, environment });
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ifunction.RestApi;

namespace ifunction.DeveloperModule
{
    /// <summary>
    /// Class EnvironmentEndpoint.
    /// </summary>
    public class EnvironmentEndpoint : ApiEndpoint
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        public DeploymentEnvironment Environment { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the azure connection string.
        /// </summary>
        /// <value>The azure connection string.</value>
        public string AzureConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the SQL connection string.
        /// </summary>
        /// <value>The SQL connection string.</value>
        public string SqlConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the mongo database connection string.
        /// </summary>
        /// <value>The mongo database connection string.</value>
        public string MongoDbConnectionString { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ifunction;
using ifunction.RestApi;

namespace ifunction.Model
{
    /// <summary>
    /// Class EnvironmentEndpointBase.
    /// </summary>
    public class EnvironmentEndpointBase : ApiEndpoint, IIdentifier
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
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the azure storage connection string.
        /// </summary>
        /// <value>The azure storage connection string.</value>
        public string AzureStorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the SQL connection string.
        /// </summary>
        /// <value>The SQL connection string.</value>
        public string SqlConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the mongo connection string.
        /// </summary>
        /// <value>The mongo connection string.</value>
        public string MongoConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }
    }
}

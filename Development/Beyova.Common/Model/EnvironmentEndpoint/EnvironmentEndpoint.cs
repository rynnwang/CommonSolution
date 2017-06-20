using System;
using Beyova.Api;

namespace Beyova
{
    /// <summary>
    /// Class EnvironmentEndpoint.
    /// </summary>
    public class EnvironmentEndpoint : ApiEndpoint, IIdentifier, IProjectBased
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
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the project key.
        /// </summary>
        /// <value>The project key.</value>
        public Guid? ProjectKey { get; set; }
    }
}
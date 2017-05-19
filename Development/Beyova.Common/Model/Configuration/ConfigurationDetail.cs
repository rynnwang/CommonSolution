using System;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class ConfigurationDetail.
    /// </summary>
    public class ConfigurationDetail
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
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the configurations.
        /// </summary>
        /// <value>The configurations.</value>
        public JToken Configurations { get; set; }
    }
}

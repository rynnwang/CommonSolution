using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class BeyovaConfigurationAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public class BeyovaConfigurationAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the remote configuration URI.
        /// </summary>
        /// <value>The remote configuration URI.</value>
        public Uri RemoteConfigurationUri { get; protected set; }

        /// <summary>
        /// Gets or sets the remote configuration token.
        /// </summary>
        /// <value>The remote configuration token.</value>
        public string RemoteConfigurationToken { get; protected set; }

        /// <summary>
        /// Gets or sets the configuration path.
        /// </summary>
        /// <value>The configuration path.</value>
        public string ConfigurationPath { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaConfigurationAttribute"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        public BeyovaConfigurationAttribute(string id, string version)
        {
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ConfigurationPath.SafeToString(RemoteConfigurationUri?.ToString());
        }
    }
}

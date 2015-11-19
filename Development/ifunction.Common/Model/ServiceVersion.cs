using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ifunction.Model
{
    /// <summary>
    /// Class ServiceVersion.
    /// </summary>
    public class ServiceVersion
    {
        /// <summary>
        /// Gets or sets the assembly version.
        /// </summary>
        /// <value>The assembly version.</value>
        public Dictionary<string, object> AssemblyVersion { get; set; }

        /// <summary>
        /// Gets or sets the server environment.
        /// </summary>
        /// <value>The server environment.</value>
        public string ServerEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the configuration belongs.
        /// </summary>
        /// <value>The configuration belongs.</value>
        public Dictionary<string, string> ConfigurationBelongs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceVersion"/> class.
        /// </summary>
        public ServiceVersion()
        {
            this.AssemblyVersion = new Dictionary<string, object>();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
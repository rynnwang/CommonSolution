using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ifunction.Model
{
    /// <summary>
    /// Class EnvironmentInfo.
    /// </summary>
    public class EnvironmentInfo
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
        /// Gets or sets the memory usage.
        /// </summary>
        /// <value>The memory usage.</value>
        public long? MemoryUsage { get; set; }

        /// <summary>
        /// Gets or sets the cpu usage.
        /// </summary>
        /// <value>The cpu usage.</value>
        public double? CpuUsage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentInfo"/> class.
        /// </summary>
        public EnvironmentInfo()
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
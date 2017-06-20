using System;
using System.IO;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class BeyovaConfigurationAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public class BeyovaConfigurationAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the configuration. Name supports wildcard, like "beyova.*.json"
        /// </summary>
        /// <value>The name of the configuration.</value>
        public string ConfigurationName { get; protected set; }

        /// <summary>
        /// Gets or sets the configuration directory.
        /// </summary>
        /// <value>The configuration directory.</value>
        public string ConfigurationDirectory { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaConfigurationAttribute" /> class.
        /// If configurationDirectory is not specified, then use <c>{BaseDirectory}/Configurations/{configurationName}</c>
        /// </summary>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <param name="configurationDirectory">The configuration directory.</param>
        public BeyovaConfigurationAttribute(string configurationName, string configurationDirectory = null)
        {
            this.ConfigurationName = configurationName;
            this.ConfigurationDirectory = configurationDirectory;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return GetConfigurationFullPath();
        }

        /// <summary>
        /// Gets the configuration full path.
        /// </summary>
        /// <returns>System.String.</returns>
        internal string GetConfigurationFullPath()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationName))
                {
                    var isWildcard = ConfigurationName.Contains(StringConstants.AsteriskChar);

                    var directory = EnvironmentCore.GetDirectory(ConfigurationDirectory, "Configurations");
                    if (directory.Exists && isWildcard)
                    {
                        return directory.GetFiles(ConfigurationName).FirstOrDefault()?.FullName;
                    }
                    else
                    {
                        return Path.Combine(directory.FullName.TrimEnd(StringConstants.BackSlash), ConfigurationName);
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }
    }
}
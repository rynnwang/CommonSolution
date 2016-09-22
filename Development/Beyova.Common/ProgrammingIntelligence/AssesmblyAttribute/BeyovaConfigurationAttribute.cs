using System;
using System.IO;
using System.Linq;
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
        /// Gets or sets the remote configuration RSA key.
        /// </summary>
        /// <value>The remote configuration RSA key.</value>
        public string RemoteConfigurationRsaKey { get; protected set; }

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
        /// If configurationDirectory is not specified, then use BaseDirectory/Configurations/{configurationName}
        /// </summary>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <param name="configurationDirectory">The configuration directory.</param>
        public BeyovaConfigurationAttribute(string configurationName, string configurationDirectory = null)
        {
            this.ConfigurationName = configurationName;
            this.ConfigurationDirectory = configurationDirectory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaConfigurationAttribute" /> class.
        /// </summary>
        /// <param name="remoteConfigurationUri">The remote configuration URI.</param>
        /// <param name="publicRsaKey">The public RSA key.</param>
        public BeyovaConfigurationAttribute(Uri remoteConfigurationUri, string publicRsaKey)
        {
            this.RemoteConfigurationRsaKey = publicRsaKey;
            this.RemoteConfigurationUri = remoteConfigurationUri;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return RemoteConfigurationUri != null ? RemoteConfigurationUri.ToString() : GetConfigurationFullPath();
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
                    var isWildcard = ConfigurationName.Contains('*');

                    var directory = EnvironmentCore.GetDirectory(ConfigurationDirectory, "Configurations");
                    if (directory.Exists && isWildcard)
                    {
                        return directory.GetFiles(ConfigurationName).FirstOrDefault()?.FullName;
                    }
                    else
                    {
                        return Path.Combine(directory.FullName.TrimEnd('\\'), ConfigurationName);
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

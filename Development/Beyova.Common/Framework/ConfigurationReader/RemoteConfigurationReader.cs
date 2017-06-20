using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Beyova.Gravity;

namespace Beyova.Configuration
{
    /// <summary>
    /// Provides access to configuration files for client applications. Reader would try read AppConfig.JSON first, then read assembly name based JSON ({AssemblyName}.JSON) to override, based on dependency order.
    /// </summary>
    internal class RemoteConfigurationReader : BaseJsonConfigurationReader
    {
        /// <summary>
        /// The _gravity client
        /// </summary>
        private GravityClient _gravityClient;

        /// <summary>
        /// Gets or sets the name of the configuration.
        /// </summary>
        /// <value>The name of the configuration.</value>
        public string ConfigurationName { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigurationReader" /> class.
        /// </summary>
        /// <param name="gravityClient">The gravity client.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        internal RemoteConfigurationReader(GravityClient gravityClient, string configurationName, bool throwException = false)
            : base(throwException)
        {
            _gravityClient = gravityClient;
            ConfigurationName = configurationName;
        }

        #region Initialization

        /// <summary>
        /// Initializes the specified throw exception.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>Dictionary&lt;System.String, ConfigurationItem&gt;.</returns>
        protected override Dictionary<string, ConfigurationItem> Initialize(bool throwException = false)
        {
            var componentAttribute = GravityShell.Host?.ComponentAttribute;
            Dictionary<string, ConfigurationItem> settingContainer = null;

            try
            {
                var remoteConfiguration = _gravityClient.RetrieveConfiguration(this.ConfigurationName)?.Configuration;
                if (remoteConfiguration != null)
                {
                    InitializeSettings(settingContainer, componentAttribute, remoteConfiguration.ToObject<ConfigurationDetail>(), null, throwException);
                    if (settingContainer.HasItem())
                    {
                        SaveConfigurationBackup(settingContainer);
                    }
                }

                return settingContainer;
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle(new
                    {
                        componentAttribute = componentAttribute?.Id
                    });
                }

                settingContainer = RestoreBackup() ?? new Dictionary<string, ConfigurationItem>();
            }

            return settingContainer;
        }

        #endregion Initialization

        /// <summary>
        /// Refreshes the settings.
        /// </summary>
        public override void RefreshSettings()
        {
            settings.Merge(Initialize(true), true);
        }

        #region Backup

        private const string backupFileName = "config.bak";

        private const string processingBackupFileName = "config.bak.processing";

        /// <summary>
        /// Saves the configuration backup.
        /// </summary>
        /// <param name="configurations">The configurations.</param>
        protected void SaveConfigurationBackup(Dictionary<string, ConfigurationItem> configurations)
        {
            if (configurations != null)
            {
                try
                {
                    var tmpPath = Path.Combine(EnvironmentCore.ApplicationBaseDirectory, processingBackupFileName);
                    var rawContent = Encoding.UTF8.GetBytes(configurations.ToJson());
                    File.WriteAllBytes(tmpPath, rawContent.EncryptR3DES());

                    var filePath = Path.Combine(EnvironmentCore.ApplicationBaseDirectory, backupFileName);
                    File.Copy(tmpPath, filePath, true);
                    File.Delete(tmpPath);
                }
                catch { }
            }
        }

        /// <summary>
        /// Restores the backup.
        /// </summary>
        /// <returns>System.Collections.Generic.Dictionary&lt;System.String, Beyova.Configuration.BaseJsonConfigurationReader.ConfigurationItem&gt;.</returns>
        protected Dictionary<string, ConfigurationItem> RestoreBackup()
        {
            try
            {
                var filePath = Path.Combine(EnvironmentCore.ApplicationBaseDirectory, backupFileName);
                if (File.Exists(filePath))
                {
                    var raw = File.ReadAllBytes(filePath);
                    var jsonString = Encoding.UTF8.GetString(raw.DecryptR3DES());
                    return jsonString.TryConvertJsonToObject<Dictionary<string, ConfigurationItem>>();
                }
            }
            catch { }

            return null;
        }

        #endregion Backup
    }
}
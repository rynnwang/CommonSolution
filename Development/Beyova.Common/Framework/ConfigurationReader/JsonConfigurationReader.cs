using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Beyova.ProgrammingIntelligence;

namespace Beyova.Configuration
{
    /// <summary>
    /// Provides access to configuration files for client applications. Reader would try read AppConfig.JSON first, then read assembly name based JSON ({AssemblyName}.JSON) to override, based on dependency order.
    /// </summary>
    public class JsonConfigurationReader : BaseJsonConfigurationReader
    {
        /// <summary>
        /// The configuration file extension
        /// </summary>
        public const string configurationFileExtension = ".json";

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigurationReader" /> class.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        public JsonConfigurationReader(bool throwException = false)
            : base(throwException)
        {
        }

        #region Initialization

        /// <summary>
        /// Initializes the specified throw exception.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>Dictionary&lt;System.String, ConfigurationItem&gt;.</returns>
        protected override Dictionary<string, ConfigurationItem> Initialize(bool throwException = false)
        {
            Dictionary<string, ConfigurationItem> result = new Dictionary<string, ConfigurationItem>();

            foreach (var one in EnvironmentCore.DescendingAssemblyDependencyChain)
            {
                result.Merge(InitializeByAssembly(one));
            }

            return result;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        protected Dictionary<string, ConfigurationItem> InitializeByAssembly(Assembly assembly, bool throwException = false)
        {
            try
            {
                if (assembly != null)
                {
                    var settingContainer = new Dictionary<string, ConfigurationItem>();

                    var beyovaConfiguration = assembly.GetCustomAttribute<BeyovaConfigurationAttribute>();
                    var beyovaComponent = assembly?.GetCustomAttribute<BeyovaComponentAttribute>();
                    var assemblyName = assembly.GetName()?.Name;

                    var configurationPath = beyovaConfiguration == null ? GetConfigurationFilePath(assemblyName) : beyovaConfiguration.GetConfigurationFullPath();

                    if (!string.IsNullOrWhiteSpace(configurationPath))
                    {
                        string jsonString = string.Empty;

                        if (File.Exists(configurationPath))
                        {
                            jsonString = File.ReadAllText(configurationPath, Encoding.UTF8);
                        }

                        if (!string.IsNullOrWhiteSpace(jsonString))
                        {
                            InitializeSettings(settingContainer, beyovaComponent, jsonString.TryConvertJsonToObject<ConfigurationDetail>(), assemblyName, throwException);
                        }
                    }

                    return settingContainer;
                }

                return new Dictionary<string, ConfigurationItem>();
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle(new { assembly = assembly?.FullName });
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion Initialization

        /// <summary>
        /// Refreshes the settings.
        /// </summary>
        public override void RefreshSettings()
        {
            settings.Merge(Initialize(true), true);
        }

        /// <summary>
        /// Gets the default configuration file path.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>System.String.</returns>
        private static string GetConfigurationFilePath(string assemblyName)
        {
            return string.IsNullOrWhiteSpace(assemblyName) ? null : Path.Combine(EnvironmentCore.ApplicationBaseDirectory, string.Format(@"Configurations\{0}.json", assemblyName));
        }

        #region Singleton

        private static JsonConfigurationReader _defaultConfigurationReader = new JsonConfigurationReader(false);

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>The default.</value>
        public static JsonConfigurationReader Default
        {
            get { return _defaultConfigurationReader; }
        }

        #endregion Singleton
    }
}
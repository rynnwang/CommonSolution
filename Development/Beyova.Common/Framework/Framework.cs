using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Beyova.Configuration;
using Beyova.Gravity;
using Beyova.ProgrammingIntelligence;

namespace Beyova
{
    /// <summary>
    /// Class Framework.
    /// </summary>
    public static class Framework
    {
        /// <summary>
        /// The configuration reader
        /// </summary>
        internal readonly static IConfigurationReader ConfigurationReader = (GravityShell.Host?.ConfigurationReader) ?? JsonConfigurationReader.Default as IConfigurationReader;

        /// <summary>
        /// The global culture resource collection
        /// </summary>
        internal static GlobalCultureResourceCollection GlobalCultureResourceCollection;

        /// <summary>
        /// The assembly version
        /// </summary>
        private static Dictionary<string, object> AssemblyVersion;

        #region Public

        /// <summary>
        /// Gets the primary SQL connection.
        /// </summary>
        /// <value>
        /// The primary SQL connection.
        /// </value>
        public static string PrimarySqlConnection
        {
            get
            {
                return GetConfiguration("SqlConnection").SafeToString(GetConfiguration("PrimarySqlConnection"));
            }
        }

        /// <summary>
        /// Sets the global default api tracking.
        /// </summary>
        /// <param name="apiTracking">The API tracking.</param>
        [Obsolete("Use BeyovaComponent Attribute in assembly to set default ApiTracking instance.")]
        public static void SetGlobalDefaultApiTracking(IApiTracking apiTracking)
        {
            if (apiTracking != null)
            {
                ApiTracking = apiTracking;
            }
        }

        /// <summary>
        /// Abouts the service.
        /// </summary>
        /// <returns>ServiceVersion.</returns>
        public static EnvironmentInfo AboutService()
        {
            try
            {
                var result = new EnvironmentInfo { AssemblyVersion = AssemblyVersion };

                result.ConfigurationBelongs = ConfigurationReader == null ? new Dictionary<string, string>() : ConfigurationReader.ConfigurationBelongs;
                result.SqlDatabaseEnvironment = ConfigurationReader == null ? string.Empty : DatabaseOperator.AboutSqlServer(ConfigurationReader.PrimarySqlConnection);
                result.MemoryUsage = SystemManagementExtension.GetProcessMemoryUsage();
                result.GCMemory = SystemManagementExtension.GetGCMemory();
                result.CpuUsage = SystemManagementExtension.GetCpuUsage();
                result.ServerName = EnvironmentCore.ServerName;
                result.IpAddress = EnvironmentCore.LocalMachineIpAddress;
                result.HostName = EnvironmentCore.LocalMachineHostName;
                result.AssemblyHash = EnvironmentCore.GetAssemblyHash();

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Gets the resource by key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="languageCompatibility">The language compatibility.</param>
        /// <returns>System.String.</returns>
        public static string GetResourceString(string resourceKey, bool languageCompatibility = true)
        {
            return string.IsNullOrWhiteSpace(resourceKey) ? string.Empty : GlobalCultureResourceCollection.GetResourceString(resourceKey);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>T.</returns>
        public static T GetConfiguration<T>(string key, T defaultValue = default(T))
        {
            return ConfigurationReader.GetConfiguration<T>(key, defaultValue);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        public static string GetConfiguration(string key, string defaultValue = null)
        {
            return ConfigurationReader.GetConfiguration(key, defaultValue);
        }

        /// <summary>
        /// Gets the configuration setting count.
        /// </summary>
        /// <value>The configuration setting count.</value>
        public static int ConfigurationSettingCount { get { return ConfigurationReader.SettingsCount; } }

        /// <summary>
        /// The API tracking
        /// </summary>
        public static IApiTracking ApiTracking { get; private set; }

        /// <summary>
        /// Gets the current culture information.
        /// </summary>
        /// <value>The current culture information.</value>
        public static CultureInfo CurrentCultureInfo
        {
            get
            {
                return ContextHelper.CurrentCultureInfo ?? GlobalCultureResourceCollection?.DefaultCultureInfo;
            }
        }

        #endregion Public

        /// <summary>
        /// Initializes static members of the <see cref="Framework"/> class.
        /// </summary>
        static Framework()
        {
            Initialize();
            ApiTracking?.LogMessage(string.Format("{0} is initialized.", EnvironmentCore.ProductName));
        }

        #region Initializes

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        private static void Initialize()
        {
            try
            {
                ApiTracking = InitializeApiTracking(EnvironmentCore.AscendingAssemblyDependencyChain);
                AssemblyVersion = InitializeAssemblyVersion();
                GlobalCultureResourceCollection = GlobalCultureResourceCollection.Instance;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Initializes the assembly version.
        /// </summary>
        /// <returns>System.Collections.Generic.Dictionary&lt;System.String, System.Object&gt;.</returns>
        private static Dictionary<string, object> InitializeAssemblyVersion()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (var one in EnvironmentCore.DescendingAssemblyDependencyChain)
            {
                var info = one.GetName();

                if (!info.IsSystemAssembly())
                {
                    var beyovaComponent = one.GetCustomAttribute<BeyovaComponentAttribute>();
                    var beyovaConfiguration = one.GetCustomAttribute<BeyovaConfigurationAttribute>();

                    result.Merge(info.Name, beyovaComponent == null ? info.Version : new
                    {
                        Version = info.Version,
                        Component = beyovaComponent.ToString(),
                        Configuration = beyovaConfiguration.SafeToString()
                    } as object);
                }
            }

            return result;
        }

        /// <summary>
        /// Initializes the API tracking.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>Beyova.IApiTracking.</returns>
        private static IApiTracking InitializeApiTracking(List<Assembly> list)
        {
            IApiTracking result = null;

            foreach (var one in list)
            {
                var componentAttribute = one.GetCustomAttribute<BeyovaComponentAttribute>();
                if (componentAttribute != null)
                {
                    result = componentAttribute.GetApiTrackingInstance();
                    if (result != null)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        #endregion Initializes
    }
}
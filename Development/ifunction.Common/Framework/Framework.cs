using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using ifunction.ApiTracking;
using ifunction.Configuration;
using ifunction.Constants;
using ifunction.Model;

namespace ifunction
{
    /// <summary>
    /// Class Framework.
    /// </summary>
    public static class Framework
    {
        /// <summary>
        /// The configuration reader
        /// </summary>
        internal static IConfigurationReader configurationReader = null;

        #region Public

        /// <summary>
        /// Abouts the service.
        /// </summary>
        /// <returns>ServiceVersion.</returns>
        public static ServiceVersion AboutService()
        {
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var result = new ServiceVersion();

                foreach (var one in assemblies)
                {
                    var info = one.GetName();
                    if (!info.Name.StartsWith("Microsoft.")
                        && !info.Name.StartsWith("System.")
                        && info.Name != "mscorlib")
                    {
                        var beyovaComponent = one.GetComponentAttribute();

                        result.AssemblyVersion.Merge(info.Name, beyovaComponent == null ? info.Version : new { Version = info.Version, Component = beyovaComponent.ToString() } as object);
                    }
                }

                result.ConfigurationBelongs = configurationReader == null ? new Dictionary<string, string>() : configurationReader.ConfigurationBelongs;
                result.ServerEnvironment = configurationReader == null ? string.Empty : DatabaseOperator.AboutSqlServer(configurationReader.SqlConnection);

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle("AboutService");
            }
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
            return configurationReader.GetConfiguration<T>(key, defaultValue);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        public static string GetConfiguration(string key, string defaultValue = null)
        {
            return configurationReader.GetConfiguration(key, defaultValue);
        }

        /// <summary>
        /// The API tracking
        /// </summary>
        public static IApiTracking ApiTracking { get; private set; }

        /// <summary>
        /// Gets or sets the operator information.
        /// </summary>
        /// <value>The operator information.</value>
        public static object OperatorInfo
        {
            get { return ThreadKeys.CurrentOperator.GetThreadData().SafeToString(); }
        }

        #endregion

        /// <summary>
        /// Initializes static members of the <see cref="Framework"/> class.
        /// </summary>
        static Framework()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private static void Initialize()
        {
            configurationReader = GetDefaultConfigurationReader();
            ApiTracking = DiagnosticFileLogger.CreateOrUpdateDiagnosticFileLogger();
        }

        /// <summary>
        /// Gets the default configuration reader.
        /// </summary>
        /// <returns>IConfigurationReader.</returns>
        private static IConfigurationReader GetDefaultConfigurationReader()
        {
            return new JsonConfigurationReader();
        }
    }
}

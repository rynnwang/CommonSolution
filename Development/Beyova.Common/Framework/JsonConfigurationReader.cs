using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using Beyova.ProgrammingIntelligence;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Configuration
{
    /// <summary>
    /// Provides access to configuration files for client applications. Reader would try read AppConfig.JSON first, then read assembly name based JSON ({AssemblyName}.JSON) to override, based on dependency order.
    /// </summary>
    public class JsonConfigurationReader : IConfigurationReader
    {
        /// <summary>
        /// The configuration file extension
        /// </summary>
        public const string configurationFileExtension = ".json";

        /// <summary>
        /// Class ConfigurationItem.
        /// </summary>
        class ConfigurationItem
        {
            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public object Value { get; set; }

            /// <summary>
            /// Gets or sets the assembly.
            /// </summary>
            /// <value>The assembly.</value>
            public string Assembly { get; set; }

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
            /// Gets or sets a value indicating whether this <see cref="ConfigurationItem"/> is encrypted.
            /// </summary>
            /// <value><c>true</c> if encrypted; otherwise, <c>false</c>.</value>
            public bool Encrypted { get; set; }

            /// <summary>
            /// Gets or sets the minimum component version required.
            /// </summary>
            /// <value>The minimum component version required.</value>
            public string MinComponentVersionRequired { get; set; }

            /// <summary>
            /// Gets or sets the maximum component version limited.
            /// </summary>
            /// <value>The maximum component version limited.</value>
            public string MaxComponentVersionLimited { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is active.
            /// </summary>
            /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
            public bool IsActive { get; set; }
        }

        #region Constants

        /// <summary>
        /// The XML attribute_ type
        /// </summary>
        public const string Attribute_Type = "Type";

        /// <summary>
        /// The attribute_ minimum component version require
        /// </summary>
        public const string Attribute_MinComponentVersionRequire = "MinComponentVersionRequired";

        /// <summary>
        /// The attribute_ maximum component version limited
        /// </summary>
        public const string Attribute_MaxComponentVersionLimited = "MaxComponentVersionLimited";

        /// <summary>
        /// The XML attribute_ version
        /// </summary>
        public const string Attribute_Version = "Version";

        /// <summary>
        /// The attribute_ name
        /// </summary>
        public const string Attribute_Name = "Name";

        /// <summary>
        /// The attribute_ environment
        /// </summary>
        public const string Attribute_Environment = "Environment";

        /// <summary>
        /// The XML attribute_ value
        /// </summary>
        public const string Attribute_Value = "Value";

        /// <summary>
        /// The attribute_ encrypted
        /// </summary>
        public const string Attribute_Encrypted = "Encrypted";

        /// <summary>
        /// The configuration key_ SQL connection
        /// </summary>
        private const string ConfigurationKey_SqlConnection = "SqlConnection";

        #endregion

        /// <summary>
        /// The settings
        /// </summary>
        private Dictionary<string, ConfigurationItem> settings = null;

        /// <summary>
        /// Gets the settings count.
        /// </summary>
        /// <value>The settings count.</value>
        public int SettingsCount
        {
            get
            {
                return settings == null ? 0 : settings.Count;
            }
        }

        /// <summary>
        /// Gets the SQL connection.
        /// </summary>
        /// <value>The SQL connection.</value>
        public string SqlConnection
        {
            get { return GetConfiguration(ConfigurationKey_SqlConnection); }
        }

        /// <summary>
        /// Gets the configuration belongs.
        /// </summary>
        /// <value>The configuration belongs.</value>
        public Dictionary<string, string> ConfigurationBelongs
        {
            get
            {
                Dictionary<string, string> result = new Dictionary<string, string>();

                foreach (var one in settings)
                {
                    result.Add(one.Key, string.Format("Assembly: {0}, Environment: {1}, Name: {2}", one.Value.Assembly, one.Value.Environment, one.Value.Name));
                }

                return result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigurationReader" /> class.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        public JsonConfigurationReader(bool throwException = false)
        {
            settings = Initialize(throwException);
        }

        #region Public method

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>T.</returns>
        public T GetConfiguration<T>(string key, T defaultValue = default(T))
        {
            ConfigurationItem configuration = null;

            if (settings.SafeTryGetValue(key, out configuration) && configuration.IsActive)
            {
                return (T)configuration.Value;
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the configuration as object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Object.</returns>
        protected object GetConfigurationAsObject(string key, object defaultValue = null)
        {
            ConfigurationItem configuration = null;
            return (settings.SafeTryGetValue(key, out configuration) && configuration.IsActive) ? configuration.Value : defaultValue;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        public string GetConfiguration(string key, string defaultValue = null)
        {
            return GetConfigurationAsObject(key, defaultValue).SafeToString();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the specified throw exception.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>Dictionary&lt;System.String, ConfigurationItem&gt;.</returns>
        private Dictionary<string, ConfigurationItem> Initialize(bool throwException = false)
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
        private Dictionary<string, ConfigurationItem> InitializeByAssembly(Assembly assembly, bool throwException = false)
        {
            try
            {
                if (assembly != null)
                {
                    var settingContainer = new Dictionary<string, ConfigurationItem>();

                    var beyovaConfiguration = assembly.GetCustomAttribute<BeyovaConfigurationAttribute>();
                    var beyovaComponent = assembly?.GetCustomAttribute<BeyovaComponentAttribute>();
                    var assemblyName = assembly.GetName()?.Name;

                    if (beyovaConfiguration != null && beyovaConfiguration.RemoteConfigurationUri != null)
                    {
                        InitializeConfigurationByRemoteUri(settingContainer, beyovaComponent, assemblyName, beyovaConfiguration, throwException);
                    }
                    else
                    {
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
                                InitializeSettings(settingContainer, beyovaComponent, jsonString, assemblyName, throwException);
                            }
                        }
                    }

                    return settingContainer;
                }

                return new Dictionary<string, ConfigurationItem>();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { assembly = assembly?.FullName });
            }
        }

        /// <summary>
        /// Initializes the settings.
        /// </summary>
        /// <param name="settingContainer">The setting container.</param>
        /// <param name="componentAttribute">The component attribute.</param>
        /// <param name="jsonString">The json string.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        private void InitializeSettings(IDictionary<string, ConfigurationItem> settingContainer, BeyovaComponentAttribute componentAttribute, string jsonString, string assemblyName, bool throwException = false)
        {
            settingContainer.Clear();

            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                try
                {
                    var root = JObject.Parse(jsonString);
                    root.CheckNullObject("root");

                    var name = root.GetValue(Attribute_Name)?.Value<string>();
                    name = name.SafeToString(root.GetValue(Attribute_Version)?.Value<string>());
                    var environment = root.GetValue(Attribute_Environment)?.Value<string>();

                    foreach (JProperty one in root.GetValue("Configurations").Children())
                    {
                        FillObjectCollection(settingContainer, componentAttribute, one, assemblyName, name, environment, throwException);
                    }
                }
                catch (Exception ex)
                {
                    var exception = ex.Handle(new { jsonString, assemblyName });
                    if (throwException)
                    {
                        throw exception;
                    }
                }
            }
        }

        /// <summary>
        /// Fills the object collection.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="componentAttribute">The component attribute.</param>
        /// <param name="itemNode">The XML node.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="configurationSourceName">Name of the configuration source.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <exception cref="System.InvalidOperationException">Failed to FillObjectCollection, Data:  + xmlNode.ToString() + \n\r</exception>
        private static void FillObjectCollection(IDictionary<string, ConfigurationItem> container, BeyovaComponentAttribute componentAttribute, JProperty itemNode, string assemblyName, string configurationSourceName, string environment, bool throwException = false)
        {
            try
            {
                container.CheckNullObject("container");
                itemNode.CheckNullObject("valueNode");

                var key = itemNode.Name;
                var valueNode = itemNode.Value;
                var typeFullName = valueNode.SelectToken(Attribute_Type).Value<string>();
                var encrypted = (valueNode.SelectToken(Attribute_Encrypted)?.Value<bool>()) ?? false;
                var minVersion = valueNode.SelectToken(Attribute_MinComponentVersionRequire)?.Value<string>();
                var maxVersion = valueNode.SelectToken(Attribute_MaxComponentVersionLimited)?.Value<string>();

                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(typeFullName))
                {
                    var objectType = ReflectionExtension.SmartGetType(typeFullName, false) ?? ReflectionExtension.SmartGetType(typeFullName, true);

                    if (objectType != null)
                    {
                        var valueJsonObject = encrypted ? DecryptObject(valueNode.SelectToken(Attribute_Value).ToObject<string>()) : valueNode.SelectToken(Attribute_Value);
                        var valueObject = valueJsonObject.ToObject(objectType);
                        container.Merge(key, new ConfigurationItem
                        {
                            Value = valueObject,
                            IsActive = IsActive(componentAttribute?.Version, minVersion, maxVersion),
                            Assembly = assemblyName,
                            MaxComponentVersionLimited = maxVersion,
                            MinComponentVersionRequired = minVersion,
                            Encrypted = encrypted,
                            Environment = environment,
                            Name = configurationSourceName
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle(data: itemNode.ToString());
                }
            }
        }

        /// <summary>
        /// Decrypts the object.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <returns>Newtonsoft.Json.Linq.JToken.</returns>
        private static JToken DecryptObject(string jsonString)
        {
            return string.IsNullOrWhiteSpace(jsonString) ? null : jsonString.R3DDecrypt3DES();
        }

        /// <summary>
        /// Encrypts the object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        private static string EncryptObject(object obj)
        {
            return obj == null ? null : obj.ToJson(false).R3DEncrypt3DES();
        }

        /// <summary>
        /// Determines whether the specified version value is active.
        /// </summary>
        /// <param name="versionValue">The version value.</param>
        /// <param name="minRequired">The minimum required.</param>
        /// <param name="maxLimited">The maximum limited.</param>
        /// <returns>System.Boolean.</returns>
        private static bool IsActive(string versionValue, string minRequired, string maxLimited)
        {
            Version version = null, min = null, max = null;

            try
            {
                version = string.IsNullOrWhiteSpace(versionValue) ? null : new Version(versionValue);
                min = string.IsNullOrWhiteSpace(minRequired) ? null : new Version(minRequired);
                max = string.IsNullOrWhiteSpace(maxLimited) ? null : new Version(maxLimited);
            }
            catch { }

            if (version == null)
            {
                return min == null && max == null;
            }
            else
            {
                if ((min != null && min > version)
                    || (max != null && max < version))
                {
                    return false;
                }
            }

            return true;
        }


        #endregion

        /// <summary>
        /// Refreshes the settings.
        /// </summary>
        public void RefreshSettings()
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

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerable<KeyValuePair<string, object>> GetValues()
        {
            var result = new Dictionary<string, object>();

            settings.Where(result, (k, v) => { return v?.IsActive ?? false; }, x => x.Value);
            return result;
        }

        #region Remote Configuration

        /// <summary>
        /// Initializes the configuration by remote URI.
        /// </summary>
        /// <param name="settingContainer">The setting container.</param>
        /// <param name="beyovaComponent">The beyova component.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="configurationAttribute">The configuration attribute.</param>
        /// <param name="throwException">The throw exception.</param>
        /// <returns>System.Collections.Generic.Dictionary&lt;System.String, Beyova.Configuration.JsonConfigurationReader.ConfigurationItem&gt;.</returns>
        private void InitializeConfigurationByRemoteUri(Dictionary<string, ConfigurationItem> settingContainer, BeyovaComponentAttribute beyovaComponent, string assemblyName, BeyovaConfigurationAttribute configurationAttribute, bool throwException)
        {
            if (configurationAttribute != null && configurationAttribute.RemoteConfigurationUri != null
                && !string.IsNullOrWhiteSpace(configurationAttribute.RemoteConfigurationRsaKey))
            {
                try
                {
                    var jsonString = configurationAttribute.RemoteConfigurationUri.SecureHttpInvoke(new object { }, configurationAttribute.RemoteConfigurationRsaKey);
                    InitializeSettings(settingContainer, beyovaComponent, jsonString, assemblyName, throwException);
                }
                catch (Exception ex)
                {
                    if (throwException)
                    {
                        throw ex.Handle(new { configurationAttribute.RemoteConfigurationUri, configurationAttribute.RemoteConfigurationRsaKey });
                    }
                }
            }
        }

        #endregion

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

        #endregion
    }
}

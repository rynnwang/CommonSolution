using System;
using System.Collections.Generic;
using System.Text;
using Beyova.ProgrammingIntelligence;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Beyova.Configuration
{
    /// <summary>
    /// Class BaseJsonConfigurationReader.
    /// </summary>
    public abstract class BaseJsonConfigurationReader : IConfigurationReader
    {
        /// <summary>
        /// Class ConfigurationItem.
        /// </summary>
        internal protected class ConfigurationItem
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
        /// The configuration key_ SQL connection
        /// </summary>
        private const string ConfigurationKey_SqlConnection = "SqlConnection";

        #endregion

        /// <summary>
        /// The settings
        /// </summary>
        protected Dictionary<string, ConfigurationItem> settings = null;

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
        /// Gets the hash.
        /// </summary>
        /// <value>The hash.</value>
        public string Hash
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var key in settings.Keys.OrderBy(x => x))
                {
                    stringBuilder.Append(key);
                    stringBuilder.Append(settings[key].Value.ToString());
                }

                return stringBuilder.ToString().ToMD5String();
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
        protected BaseJsonConfigurationReader(bool throwException = false)
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
        protected abstract Dictionary<string, ConfigurationItem> Initialize(bool throwException = false);

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
        protected static void FillObjectCollection(IDictionary<string, ConfigurationItem> container, BeyovaComponentAttribute componentAttribute, JProperty itemNode, string assemblyName, string configurationSourceName, string environment, bool throwException = false)
        {
            try
            {
                container.CheckNullObject(nameof(container));
                itemNode.CheckNullObject(nameof(itemNode));

                var key = itemNode.Name;
                var valueNode = itemNode.Value.ToObject<ConfigurationRawItem>();
                var typeFullName = valueNode.Type;
                var encrypted = (valueNode.Encrypted) ?? false;
                var minVersion = valueNode.MinComponentVersionRequire;
                var maxVersion = valueNode.MaxComponentVersionLimited;

                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(typeFullName))
                {
                    var objectType = ReflectionExtension.SmartGetType(typeFullName, false) ?? ReflectionExtension.SmartGetType(typeFullName, true);

                    if (objectType != null)
                    {
                        var valueJsonObject = encrypted ? DecryptObject(valueNode.Value.ToObject<string>()) : valueNode.Value;
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
        /// Initializes the settings.
        /// </summary>
        /// <param name="settingContainer">The setting container.</param>
        /// <param name="componentAttribute">The component attribute.</param>
        /// <param name="configurationDetail">The configuration detail.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        protected void InitializeSettings(IDictionary<string, ConfigurationItem> settingContainer, BeyovaComponentAttribute componentAttribute, ConfigurationDetail configurationDetail, string assemblyName, bool throwException = false)
        {
            settingContainer.Clear();

            try
            {
                configurationDetail.CheckNullObject(nameof(configurationDetail));

                var name = configurationDetail.Name.SafeToString(configurationDetail.Version);
                var environment = configurationDetail.Environment;

                foreach (JProperty one in configurationDetail.Configurations.Children())
                {
                    FillObjectCollection(settingContainer, componentAttribute, one, assemblyName, name, environment, throwException);
                }
            }
            catch (Exception ex)
            {
                var exception = ex.Handle(new { configurationDetail, assemblyName });
                if (throwException)
                {
                    throw exception;
                }
            }
        }

        /// <summary>
        /// Decrypts the object.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <returns>Newtonsoft.Json.Linq.JToken.</returns>
        protected static JToken DecryptObject(string jsonString)
        {
            return string.IsNullOrWhiteSpace(jsonString) ? null : jsonString.DecryptR3DES();
        }

        /// <summary>
        /// Encrypts the object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        protected static string EncryptObject(object obj)
        {
            return obj == null ? null : obj.ToJson(false).EncryptR3DES();
        }

        /// <summary>
        /// Determines whether the specified version value is active.
        /// </summary>
        /// <param name="versionValue">The version value.</param>
        /// <param name="minRequired">The minimum required.</param>
        /// <param name="maxLimited">The maximum limited.</param>
        /// <returns>System.Boolean.</returns>
        protected static bool IsActive(string versionValue, string minRequired, string maxLimited)
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
        public abstract void RefreshSettings();

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
    }
}

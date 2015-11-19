using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ifunction.Configuration
{
    /// <summary>
    /// Provides access to configuration files for client applications. Reader would try read AppConfig.JSON first, then read assembly name based JSON ({AssemblyName}.JSON) to override, based on dependency order.
    /// </summary>
    public class JsonConfigurationReader : IConfigurationReader
    {
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
            /// Gets or sets the source.
            /// </summary>
            /// <value>The source.</value>
            public string Source { get; set; }
        }

        #region Constants

        /// <summary>
        /// The XML node_ root node name
        /// </summary>
        public const string Node_RootNodeName = "Configurations";

        /// <summary>
        /// The XML node_ object item
        /// </summary>
        public const string Node_ObjectItem = "Object";

        /// <summary>
        /// The XML node_ method item
        /// </summary>
        public const string Node_MethodItem = "Method";

        /// <summary>
        /// The XML attribute_ type
        /// </summary>
        public const string Attribute_Type = "Type";

        /// <summary>
        /// The XML attribute_ version
        /// </summary>
        public const string Attribute_Version = "Version";

        /// <summary>
        /// The XML attribute_ value
        /// </summary>
        public const string Attribute_Value = "Value";

        /// <summary>
        /// The XML attribute_ name
        /// </summary>
        public const string Attribute_Name = "Name";

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
                    result.Add(one.Key, one.Value.Source);
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

            if (settings.SafeTryGetValue(key, out configuration))
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
            return settings.SafeTryGetValue(key, out configuration) ? configuration.Value : defaultValue;
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
            var sorted = ReflectionExtension.GetAppDomainAssemblies().GetAssemblyDependencyChain();
            Dictionary<string, ConfigurationItem> result = InitializeByAssembly(null);

            foreach (var one in sorted)
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
            var settingContainer = new Dictionary<string, ConfigurationItem>();
            var assemblyName = (assembly != null) ? assembly.GetName().Name : null;

            var configurationPath = GetConfigurationFilePath(assemblyName);
            string jsonString = string.Empty;

            if (File.Exists(configurationPath))
            {
                jsonString = File.ReadAllText(configurationPath, Encoding.UTF8);
            }

            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                InitializeSettings(settingContainer, jsonString, assembly == null ? "AppConfig" : assemblyName, throwException);
            }

            return settingContainer;
        }

        /// <summary>
        /// Initializes the settings.
        /// </summary>
        /// <param name="settingContainer">The setting container.</param>
        /// <param name="jsonString">The json string.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        private void InitializeSettings(IDictionary<string, ConfigurationItem> settingContainer, string jsonString, string assemblyName, bool throwException = false)
        {
            settingContainer.Clear();

            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                try
                {
                    var root = JObject.Parse(jsonString);
                    root.CheckNullObject("root");

                    var version = root.GetValue(Attribute_Version).Value<string>();

                    foreach (JProperty one in root.GetValue("Configurations").Children())
                    {
                        FillObjectCollection(settingContainer, one, string.Format("{0}({1})", assemblyName, version), throwException);
                    }
                }
                catch (Exception ex)
                {
                    var exception = ex.Handle("InitializeSettings", new { jsonString, assemblyName });
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
        /// <param name="itemNode">The XML node.</param>
        /// <param name="valueSource">The value source.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <exception cref="System.InvalidOperationException">Failed to FillObjectCollection, Data:  + xmlNode.ToString() + \n\r</exception>
        private static void FillObjectCollection(IDictionary<string, ConfigurationItem> container, JProperty itemNode, string valueSource, bool throwException = false)
        {
            try
            {
                container.CheckNullObject("container");
                itemNode.CheckNullObject("valueNode");

                var key = itemNode.Name;
                var valueNode = itemNode.Value;
                var typeFullName = valueNode.SelectToken(Attribute_Type).Value<string>();

                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(typeFullName))
                {
                    var objectType = ReflectionExtension.SmartGetType(typeFullName);

                    if (objectType != null)
                    {
                        var valueObject = valueNode.SelectToken(Attribute_Value).ToObject(objectType);
                        container.Merge(key, new ConfigurationItem { Value = valueObject, Source = valueSource });
                    }
                }
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle("FillObjectCollection", data: itemNode.ToString());
                }
            }
        }

        #endregion

        /// <summary>
        /// Refreshes the settings.
        /// </summary>
        public void RefreshSettings()
        {
            settings = Initialize(true);
        }

        /// <summary>
        /// Gets the default configuration file path.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>System.String.</returns>
        private static string GetConfigurationFilePath(string assemblyName = null)
        {
            return Path.Combine(EnvironmentCore.ApplicationBaseDirectory, string.Format(@"Configurations\{0}.json", assemblyName.SafeToString("AppConfig")));
        }

        #region Update operations

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            var root = new
            {
                Version = DateTime.UtcNow.ToString("yyyyMMdd-HH-mm-ss"),
                Configurations = new Dictionary<string, object>()
            };

            foreach (var one in this.settings)
            {
                root.Configurations.Add(one.Key, new
                {
                    Type = one.Value.GetType().ToString(),
                    Value = one.Value
                });
            }

            File.WriteAllText(GetConfigurationFilePath(), JsonConvert.SerializeObject(root));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerable<KeyValuePair<string, object>> GetValues()
        {
            return settings.Select(x => { return new KeyValuePair<string, object>(x.Key, x.Value.Value); });
        }

        #endregion
    }
}

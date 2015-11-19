using System.Collections.Generic;
using System.Xml.Linq;

namespace ifunction
{
    /// <summary>
    /// Interface IConfigurationReader
    /// </summary>
    public interface IConfigurationReader
    {
        /// <summary>
        /// Gets the settings count.
        /// </summary>
        /// <value>The settings count.</value>
        int SettingsCount { get; }

        /// <summary>
        /// Gets the SQL connection.
        /// </summary>
        /// <value>The SQL connection.</value>
        string SqlConnection { get; }

        /// <summary>
        /// Gets the configuration belongs.
        /// </summary>
        /// <value>The configuration belongs.</value>
        Dictionary<string, string> ConfigurationBelongs { get; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>T.</returns>
        T GetConfiguration<T>(string key, T defaultValue = default(T));

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        string GetConfiguration(string key, string defaultValue = null);

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <returns>IEnumerable&lt;KeyValuePair&lt;System.String, System.Object&gt;&gt;.</returns>
        IEnumerable<KeyValuePair<string, object>> GetValues();

        /// <summary>
        /// Refreshes the settings.
        /// </summary>
        void RefreshSettings();
    }
}

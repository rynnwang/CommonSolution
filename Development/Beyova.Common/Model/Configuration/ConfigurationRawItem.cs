using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class ConfigurationRawItem
    /// </summary>
    public class ConfigurationRawItem
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ConfigurationRawItem"/> is encrypted.
        /// </summary>
        /// <value><c>null</c> if [encrypted] contains no value, <c>true</c> if [encrypted]; otherwise, <c>false</c>.</value>
        public bool? Encrypted { get; set; }

        /// <summary>
        /// Gets or sets the minimum component version require.
        /// </summary>
        /// <value>The minimum component version require.</value>
        public string MinComponentVersionRequire { get; set; }

        /// <summary>
        /// Gets or sets the maximum component version limited.
        /// </summary>
        /// <value>The maximum component version limited.</value>
        public string MaxComponentVersionLimited { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public JToken Value { get; set; }
    }
}
using System.Collections.Generic;
using Beyova.Api;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class RuntimeApiOperationParameters.
    /// </summary>
    public class RuntimeApiOperationParameters
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is token required.
        /// </summary>
        /// <value><c>true</c> if this instance is token required; otherwise, <c>false</c>.</value>
        public bool IsTokenRequired { get; internal set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is data sensitive.
        /// </summary>
        /// <value><c>true</c> if this instance is data sensitive; otherwise, <c>false</c>.</value>
        public bool IsDataSensitive { get; internal set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public IDictionary<string, ApiPermission> Permissions { get; internal set; }

        /// <summary>
        /// Gets or sets the header keys.
        /// </summary>
        /// <value>The header keys.</value>
        public List<string> CustomizedHeaderKeys { get; internal set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; internal set; }

        /// <summary>
        /// Gets or sets the entity synchronization mode.
        /// </summary>
        /// <value>The entity synchronization mode.</value>
        public EntitySynchronizationModeAttribute EntitySynchronizationMode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeRoute" /> class.
        /// </summary>
        public RuntimeApiOperationParameters() { }
    }
}

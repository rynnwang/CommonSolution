using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class SandboxSetting.
    /// </summary>
    public class SandboxSetting
    {
        /// <summary>
        /// Gets or sets the application base directory.
        /// </summary>
        /// <value>The application base directory.</value>
        public string ApplicationBaseDirectory { get; set; }

        /// <summary>
        /// Gets or sets the assembly name list to load. Names do NOT need to include ".dll" extension.
        /// </summary>
        /// <value>The assembly name list to load.</value>
        public List<string> AssemblyNameListToLoad { get; set; }

        /// <summary>
        /// Gets or sets the external assembly to load.
        /// </summary>
        /// <value>The external assembly to load.</value>
        public Dictionary<string, byte[]> ExternalAssemblyToLoad { get; set; }
    }
}
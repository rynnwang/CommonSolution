using System;
using System.Runtime.Serialization;

namespace Beyova.Model
{
    /// <summary>
    /// Class ClientEnvironment
    /// </summary>
    public class ClientEnvironment
    {
        /// <summary>
        /// Gets or sets Value indicating it is device.
        /// </summary>
        /// <value>Value indicating it is device.</value>
        public DeviceType Device
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Value indicating it is platform.
        /// </summary>
        /// <value>Value indicating it is platform.</value>
        public PlatformType Platform
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Value indicating it is user agent.
        /// </summary>
        /// <value>Value indicating it is user agent.</value>
        public string UserAgent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Value indicating it is application version.
        /// e.g.: 1.2.3.123, 1.2.3
        /// </summary>
        /// <value>Value indicating it is application version.</value>
        public string AppVersion
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class Heartbeat.
    /// </summary>
    public class Heartbeat : MachineHealth
    {
        /// <summary>
        /// Gets or sets the name of the configuration.
        /// </summary>
        /// <value>The name of the configuration.</value>
        public string ConfigurationName { get; set; }
    }
}

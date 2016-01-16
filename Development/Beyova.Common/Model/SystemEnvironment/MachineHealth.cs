using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova
{
    /// <summary>
    /// Class MachineHealth.
    /// </summary>
    public class MachineHealth
    {
        /// <summary>
        /// Gets or sets the memory usage.
        /// </summary>
        /// <value>The memory usage.</value>
        public long? MemoryUsage { get; set; }

        /// <summary>
        /// Gets or sets the total memory.
        /// </summary>
        /// <value>The total memory.</value>
        public long? TotalMemory { get; set; }

        /// <summary>
        /// Gets or sets the cpu usage. Unit: %.
        /// </summary>
        /// <value>The cpu usage.</value>
        public double? CpuUsage { get; set; }

        /// <summary>
        /// Gets or sets the disk usages.
        /// </summary>
        /// <value>The disk usages.</value>
        public List<DiskDriveInfo> DiskUsages { get; set; }

        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>The name of the server.</value>
        public string ServerName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineHealth"/> class.
        /// </summary>
        public MachineHealth()
        {
            this.DiskUsages = new List<DiskDriveInfo>();
        }
    }
}

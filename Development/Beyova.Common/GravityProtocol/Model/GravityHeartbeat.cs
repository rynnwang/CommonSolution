namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityHeartbeat.
    /// </summary>
    internal class GravityHeartbeat
    {
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the name of the machine.
        /// </summary>
        /// <value>The name of the machine.</value>
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the memory usage. Unit: Byte
        /// </summary>
        /// <value>The memory usage.</value>
        public long? MemoryUsage { get; set; }

        /// <summary>
        /// Gets or sets the gc memory.
        /// </summary>
        /// <value>The gc memory.</value>
        public long? GCMemory { get; set; }

        /// <summary>
        /// Gets or sets the cpu usage. Unit: %.
        /// </summary>
        /// <value>The cpu usage.</value>
        public double? CpuUsage { get; set; }

        /// <summary>
        /// Gets or sets the assembly hash.
        /// </summary>
        /// <value>The assembly hash.</value>
        public string AssemblyHash { get; set; }
    }
}

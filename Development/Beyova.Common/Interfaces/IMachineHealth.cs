namespace Beyova
{
    /// <summary>
    /// Interface IMachineHealth
    /// </summary>
    public interface IMachineHealth
    {
        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>The name of the server.</value>
        string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>The name of the host.</value>
        string HostName { get; set; }

        /// <summary>
        /// Gets or sets the cpu usage.
        /// </summary>
        /// <value>The cpu usage.</value>
        double? CpuUsage { get; set; }

        /// <summary>
        /// Gets or sets the memory usage.
        /// </summary>
        /// <value>The memory usage.</value>
        long? MemoryUsage { get; set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the total memory.
        /// </summary>
        /// <value>The total memory.</value>
        long? TotalMemory { get; set; }
    }
}
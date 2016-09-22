using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.ExceptionSystem;

namespace Beyova.Diagnostic
{
    /// <summary>
    /// Class ServiceHealth.
    /// </summary>
    public class ServiceHealth : IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        /// <value>The destination.</value>
        public string Destination { get; set; }

        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>The name of the server.</value>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the cpu usage.
        /// </summary>
        /// <value>The cpu usage.</value>
        public double? CpuUsage { get; set; }

        /// <summary>
        /// Gets or sets the memory usage.
        /// </summary>
        /// <value>The memory usage.</value>
        public double? MemoryUsage { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public ExceptionInfo Exception { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        public DateTime? CreatedStamp { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHealth" /> class.
        /// </summary>
        public ServiceHealth()
        {
            this.Key = Guid.NewGuid();
            this.CreatedStamp = DateTime.UtcNow;
        }
    }
}

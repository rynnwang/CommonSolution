using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Beyova.Model
{
    /// <summary>
    /// Class SystemMemory.
    /// </summary>
    public class SystemMemory
    {
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public double Total { get; set; }

        /// <summary>
        /// Gets or sets the available.
        /// </summary>
        /// <value>The available.</value>
        public double Available { get; set; }
    }
}
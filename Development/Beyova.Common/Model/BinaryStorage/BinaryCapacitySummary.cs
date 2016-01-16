using Beyova;

namespace Beyova.BinaryStorage
{
    /// <summary>
    /// Class BinaryCapacitySummary.
    /// </summary>
    public class BinaryCapacitySummary
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        public string Container { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the total size.
        /// </summary>
        /// <value>
        /// The total size.
        /// </value>
        public long TotalSize { get; set; }
    }
}

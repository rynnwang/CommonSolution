using System;

namespace ifunction.BinaryStorage
{
    /// <summary>
    /// Class BinaryCapacityCriteria.
    /// </summary>
    public class BinaryCapacityCriteria
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
        public Guid? OwnerKey { get; set; }
    }
}

using System;
using System.Runtime.Serialization;
using Beyova;

namespace Beyova.BinaryStorage
{
    /// <summary>
    /// Class BinaryStorageMetaData.
    /// </summary>
    public class BinaryStorageMetaData : BinaryStorageMetaBase
    {
        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>The hash.</value>
        
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        
        public Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        
        public DateTime? CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the last updated stamp.
        /// </summary>
        /// <value>The last updated stamp.</value>
        
        public DateTime? LastUpdatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        
        public BinaryStorageState State { get; set; }
    }
}

using System;

namespace Beyova
{
    /// <summary>
    /// Class BinaryStorageMetaData.
    /// </summary>
    public class BinaryStorageMetaData : BinaryStorageMetaBase, IOwnerIdentifiable
    {
        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>The hash.</value>
        public string Hash { get; set; }

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

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        public Guid? OwnerKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageMetaData"/> class.
        /// </summary>
        public BinaryStorageMetaData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageMetaBase" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public BinaryStorageMetaData(BinaryStorageIdentifier identifier)
        {
            if (identifier != null)
            {
                this.Container = identifier.Container;
                this.Identifier = identifier.Identifier;
            }
        }
    }
}
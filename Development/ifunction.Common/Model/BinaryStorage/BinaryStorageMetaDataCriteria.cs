using System;
using System.Runtime.Serialization;

namespace ifunction.BinaryStorage
{
    /// <summary>
    /// Class BinaryStorageMetaDataCriteria.
    /// </summary>
    public class BinaryStorageMetaDataCriteria : BinaryStorageIdentifier
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>The hash.</value>
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the MIME.
        /// <remarks>
        /// http://www.w3.org/wiki/Evolution/MIME
        /// </remarks>
        /// </summary>
        /// <value>The MIME.</value>
        public string Mime { get; set; }

        /// <summary>
        /// Gets or sets the minimum length.
        /// </summary>
        /// <value>The minimum length.</value>
        public long? MinLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>The maximum length.</value>
        public long? MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the minimum width.
        /// </summary>
        /// <value>The minimum width.</value>
        public int? MinWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum width.
        /// </summary>
        /// <value>The maximum width.</value>
        public int? MaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the minimum height.
        /// </summary>
        /// <value>The minimum height.</value>
        public int? MinHeight { get; set; }

        /// <summary>
        /// Gets or sets the maximum height.
        /// </summary>
        /// <value>The maximum height.</value>
        public int? MaxHeight { get; set; }

        /// <summary>
        /// Gets or sets the minimum duration.
        /// </summary>
        /// <value>The minimum duration.</value>
        public int? MinDuration { get; set; }

        /// <summary>
        /// Gets or sets the maximum duration.
        /// </summary>
        /// <value>The maximum duration.</value>
        public int? MaxDuration { get; set; }

        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        public Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets to stamp.
        /// </summary>
        /// <value>To stamp.</value>
        public DateTime? ToStamp { get; set; }

        /// <summary>
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>From stamp.</value>
        public DateTime? FromStamp { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public BinaryStorageState? State { get; set; }
    }
}

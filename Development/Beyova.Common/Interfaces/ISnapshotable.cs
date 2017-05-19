using System;
using Beyova;

namespace Beyova
{
    /// <summary>
    /// Interface ISnapshotable
    /// </summary>
    public interface ISnapshotable : IIdentifier
    {
        #region Properties

        /// <summary>
        /// Gets or sets the snapshot key.
        /// </summary>
        /// <value>The snapshot key.</value>
        Guid? SnapshotKey { get; set; }

        #endregion
    }
}

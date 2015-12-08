using System;

namespace Beyova.Utility
{
    /// <summary>
    /// Enum DiffResult
    /// </summary>
    public enum DiffResult
    {
        /// <summary>
        /// Value indicating it is no change
        /// </summary>
        NoChange = 0,
        /// <summary>
        /// Value indicating it is replace
        /// </summary>
        Replace,
        /// <summary>
        /// Value indicating it is source deleted
        /// </summary>
        SourceDeleted,
        /// <summary>
        /// Value indicating it is destination added
        /// </summary>
        DestinationAdded
    }
}

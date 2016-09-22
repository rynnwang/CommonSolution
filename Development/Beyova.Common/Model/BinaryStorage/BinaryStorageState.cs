using System.Runtime.Serialization;

namespace Beyova
{
    /// <summary>
    /// Enum BinaryStorageState
    /// </summary>
    public enum BinaryStorageState
    {
        /// <summary>
        /// Value indicating that it is undefined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Value indicating that it is pending to commit
        /// </summary>
        CommitPending = 1,
        /// <summary>
        /// Value indicating that it is committed
        /// </summary>
        Committed = 2,
        /// <summary>
        /// Value indicating that it is pending to delete
        /// </summary>
        DeletePending = 3,
        /// <summary>
        /// Value indicating that it is deleted
        /// </summary>
        Deleted = 4,
        /// <summary>
        /// Value indicating that it is disabled
        /// </summary>
        Disabled = 5,
        /// <summary>
        /// Value indicating that it is invalid
        /// <remarks>It is used when binary summary is created, but no binary uploaded when doing commit.</remarks>
        /// </summary>
        Invalid = 6,
        /// <summary>
        /// Value indicating that it is duplicated
        /// <remarks>It is used when a binary is uploaded and commit, whose hash and length are exactly same as which is already in system.</remarks>
        /// </summary>
        Duplicated = 7
    }
}

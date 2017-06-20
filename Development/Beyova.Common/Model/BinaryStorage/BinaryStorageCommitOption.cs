namespace Beyova
{
    /// <summary>
    /// Enum BinaryStorageCommitOption
    /// </summary>
    public enum BinaryStorageCommitOption
    {
        /// <summary>
        /// Value indicating that it is default
        /// </summary>
        Default = 0,

        /// <summary>
        /// Value indicating that it is allow duplicated instance
        /// </summary>
        AllowDuplicatedInstance = 1,

        /// <summary>
        /// Value indicating that it is share duplicated instance
        /// </summary>
        ShareDuplicatedInstance = 2
    }
}
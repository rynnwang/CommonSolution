namespace Beyova
{
    /// <summary>
    /// Class BinaryStorageCommitRequest.
    /// </summary>
    public class BinaryStorageCommitRequest : BinaryStorageIdentifier
    {
        /// <summary>
        /// Gets or sets the commit option.
        /// </summary>
        /// <value>The commit option.</value>
        public BinaryStorageCommitOption? CommitOption { get; set; }
    }
}
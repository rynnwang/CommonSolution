namespace Beyova
{
    /// <summary>
    /// Enum TransactionState
    /// </summary>
    public enum TransactionState
    {
        /// <summary>
        ///     The value indicating that transaction is none.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The value indicating that transaction is created.
        /// </summary>
        Created = 1,

        /// <summary>
        ///  The value indicating that transaction is pending.
        /// </summary>
        Pending = 2,

        /// <summary>
        ///     The value indicating that transaction is in process.
        /// </summary>
        InProcess = 3,

        /// <summary>
        ///     The value indicating that transaction is completed.
        /// </summary>
        Completed = 4,

        /// <summary>
        ///     The value indicating that transaction is failed.
        /// </summary>
        Failed = 5,

        /// <summary>
        ///     The value indicating that transaction is canceled.
        /// </summary>
        Canceled = 6
    }
}
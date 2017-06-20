namespace Beyova
{
    /// <summary>
    /// Enum WorkflowState
    /// </summary>
    public enum WorkflowState
    {
        /// <summary>
        ///     The value indicating that object or operation is none.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The value indicating that object or operation is pending.
        /// </summary>
        Pending = 1,

        /// <summary>
        ///     The value indicating that object or operation is approved.
        /// </summary>
        Approved = 2,

        /// <summary>
        ///     The value indicating that object or operation is rejected
        /// </summary>
        Rejected = 3,

        /// <summary>
        ///     The value indicating that operation is in process.
        /// </summary>
        InProcess = 4,

        /// <summary>
        ///     The value indicating that object or operation is rejected.
        /// </summary>
        Completed = 5
    }
}
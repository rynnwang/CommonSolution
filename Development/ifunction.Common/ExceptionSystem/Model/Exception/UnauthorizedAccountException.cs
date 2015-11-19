namespace ifunction.ExceptionSystem
{
    /// <summary>
    /// Class UnauthorizedAccountException.
    /// </summary>
    public class UnauthorizedAccountException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedOperationException" /> class.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        public UnauthorizedAccountException(string reason, object data = null, string hintMessage = null)
            : base(string.Format("Failed to authenticate account caused by [{0}].", reason), ExceptionCode.MajorCode.UnauthorizedOperation, reason.SafeToString("Account"), parameterData: data, hintMessage: hintMessage)
        {
        }

        #endregion
    }
}

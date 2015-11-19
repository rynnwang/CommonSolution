namespace ifunction.ExceptionSystem
{
    /// <summary>
    /// </summary>
    public class UnauthorizedTokenException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedTokenException" /> class.
        /// </summary>
        /// <param name="operatorIdentity">The operator identity.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        public UnauthorizedTokenException(string operatorIdentity, object data = null, string hintMessage = null)
            : base("Unauthorized token or token expired.", ExceptionCode.MajorCode.UnauthorizedOperation, "Token", operatorIdentifier: operatorIdentity, parameterData: data, hintMessage: hintMessage)
        {
        }

        #endregion
    }
}

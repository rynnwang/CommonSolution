using System;

namespace ifunction.ExceptionSystem
{
    /// <summary>
    /// Class for unauthorized operation exception
    /// </summary>
    public class UnauthorizedOperationException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedOperationException" /> class.
        /// </summary>
        /// <param name="operationIdentity">The operation identity.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="reason">The minor code.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        public UnauthorizedOperationException(string operationIdentity, string operatorIdentifier, string reason = null, object data = null, string hintMessage = null)
            : base(string.Format("Unauthorized action on [{0}].", operationIdentity), ExceptionCode.MajorCode.UnauthorizedOperation, reason.SafeToString("Operation"), operatorIdentifier: operatorIdentifier, parameterData: data, hintMessage: hintMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedOperationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        internal UnauthorizedOperationException(string message, string operatorIdentifier, string minor, Exception innerException, object data, string hintMessage = null)
            : base(message, new ExceptionCode { Major = ExceptionCode.MajorCode.UnauthorizedOperation, Minor = minor }, operatorIdentifier: operatorIdentifier, parameterData: data, hintMessage: hintMessage)
        {
        }

        #endregion
    }
}

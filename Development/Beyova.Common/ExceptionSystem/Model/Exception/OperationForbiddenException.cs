using System;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class OperationForbiddenException.
    /// </summary>
    public class OperationForbiddenException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationForbiddenException" /> class.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        public OperationForbiddenException(string actionName, string reason, Exception innerException = null, string operatorIdentifier = null, object data = null, string hintMessage = null)
            : base(string.Format("Operation [{0}] is forbidden. Reason: {1}", actionName, reason), ExceptionCode.MajorCode.OperationForbidden, reason, innerException, operatorIdentifier, data, hintMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationForbiddenException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        internal OperationForbiddenException(string message, string operatorIdentifier, string minor, Exception innerException, object data, string hintMessage = null)
            : base(message, new ExceptionCode { Major = ExceptionCode.MajorCode.OperationForbidden, Minor = minor }, operatorIdentifier: operatorIdentifier, parameterData: data, hintMessage: hintMessage)
        {
        }

        #endregion
    }
}

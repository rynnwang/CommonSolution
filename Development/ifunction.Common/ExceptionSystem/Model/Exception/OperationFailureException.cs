using System;

namespace ifunction.ExceptionSystem
{
    /// <summary>
    /// Class OperationFailureException.
    /// </summary>
    public class OperationFailureException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationFailureException" /> class.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="hintMessage">The hint message.</param>
        public OperationFailureException(string actionName, Exception innerException = null, object data = null, string operatorIdentifier = null, string minor = null, string hintMessage = null)
            : base(string.Format("Failed to operate to [{0}].", actionName), new ExceptionCode { Major = ExceptionCode.MajorCode.OperationFailure, Minor = minor }, innerException, operatorIdentifier, data, hintMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationFailureException" /> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="minor">The minor.</param>
        public OperationFailureException(Exception innerException, object data, string minor = null)
            : base(innerException.Message, new ExceptionCode { Major = ExceptionCode.MajorCode.OperationFailure, Minor = minor }, innerException, null, data)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationFailureException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        internal OperationFailureException(string message, string operatorIdentifier, string minor, Exception innerException, object data)
            : base(message, new ExceptionCode { Major = ExceptionCode.MajorCode.OperationFailure, Minor = minor }, operatorIdentifier: operatorIdentifier, parameterData: data)
        {
        }

        #endregion
    }
}

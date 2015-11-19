using System;

namespace ifunction.ExceptionSystem
{
    /// <summary>
    /// Class InvalidObjectException.
    /// </summary>
    public class InvalidObjectException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidObjectException" /> class.
        /// </summary>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="hintMessage">The hint message.</param>
        public InvalidObjectException(string objectIdentity, Exception innerException = null, object data = null, string reason = null, string hintMessage = null)
            : base(string.Format("[{0}] is invalid.", objectIdentity), new ExceptionCode { Major = ExceptionCode.MajorCode.NullOrInvalidValue, Minor = reason.SafeToString("InvalidObject") }, innerException, null, data, hintMessage: hintMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidObjectException" /> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="hintMessage">The hint message.</param>
        public InvalidObjectException(Exception innerException, object data = null, string reason = null, string hintMessage = null)
            : base(innerException?.Message.SafeToString("Invalid object."), new ExceptionCode { Major = ExceptionCode.MajorCode.NullOrInvalidValue, Minor = reason.SafeToString("InvalidObject") }, innerException, null, data, hintMessage: hintMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidObjectException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        internal InvalidObjectException(string message, string operatorIdentifier, string reason, Exception innerException, object data, string hintMessage = null)
            : base(message, new ExceptionCode { Major = ExceptionCode.MajorCode.NullOrInvalidValue, Minor = reason }, operatorIdentifier: operatorIdentifier, parameterData: data, hintMessage: hintMessage)
        {
        }

        #endregion
    }
}

using System;

namespace ifunction.ExceptionSystem
{
    /// <summary>
    /// Class InitializationFailureException.
    /// </summary>
    public class InitializationFailureException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializationFailureException" /> class.
        /// </summary>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        public InitializationFailureException(string objectIdentity, Exception innerException = null, string minor = null, object data = null, string hintMessage = null)
            : base(string.Format("Failed to initialize [{0}].", objectIdentity), new ExceptionCode { Major = ExceptionCode.MajorCode.ServiceUnavailable, Minor = minor.SafeToString("Initialize") }, innerException, null, data, hintMessage: hintMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializationFailureException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        internal InitializationFailureException(string message, string operatorIdentifier, string minor, Exception innerException, object data, string hintMessage = null)
            : base(message, new ExceptionCode { Major = ExceptionCode.MajorCode.ServiceUnavailable, Minor = minor }, operatorIdentifier: operatorIdentifier, parameterData: data, hintMessage: hintMessage)
        {
        }

        #endregion
    }
}

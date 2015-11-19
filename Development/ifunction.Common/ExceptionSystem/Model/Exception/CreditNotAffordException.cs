using System;

namespace ifunction.ExceptionSystem
{
    /// <summary>
    /// Class CreditNotAffordException.
    /// </summary>
    public class CreditNotAffordException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditNotAffordException" /> class.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceIdentifier">The resource identifier.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        public CreditNotAffordException(string resourceName, string resourceIdentifier, string operatorIdentifier, Exception innerException = null, string minor = null, object data = null, string hintMessage = null)
            : base(string.Format("Credit is not afford for resource [{0}] at [{1}].", resourceName, resourceIdentifier), new ExceptionCode { Major = ExceptionCode.MajorCode.CreditNotAfford, Minor = minor.SafeToString(resourceName) }, innerException, operatorIdentifier, data, hintMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditNotAffordException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        internal CreditNotAffordException(string message, string operatorIdentifier, string minor, Exception innerException, object data, string hintMessage = null)
            : base(message, new ExceptionCode { Major = ExceptionCode.MajorCode.CreditNotAfford, Minor = minor }, operatorIdentifier: operatorIdentifier, parameterData: data, hintMessage: hintMessage)
        {
        }

        #endregion
    }
}

using System;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class RemoteServiceOperationFailureException.
    /// </summary>
    public class RemoteServiceOperationFailureException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteServiceOperationFailureException" /> class.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        /// <param name="action">The action.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        public RemoteServiceOperationFailureException(string serviceName, string serverIdentifier, string action, Exception innerException = null, string operatorIdentifier = null, object data = null, string hintMessage = null)
            : base(string.Format("Failed to call remote service [{0}] at [{1}] for action [{2}]", serviceName, serverIdentifier, action), ExceptionCode.MajorCode.ServiceUnavailable, serviceName, innerException, operatorIdentifier, data, hintMessage)
        {
        }

        #endregion
    }
}

using System;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
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
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        public OperationFailureException(Exception innerException = null, object data = null, string minor = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base(string.Format("Failed to operate to [{0}].", scene?.MethodName), new ExceptionCode { Major = ExceptionCode.MajorCode.OperationFailure, Minor = minor }, innerException, data, hint, scene)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationFailureException" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="createdStamp">The created stamp.</param>
        /// <param name="message">The message.</param>
        /// <param name="scene">The scene.</param>
        /// <param name="code">The code.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="operatorCredential">The operator credential.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        internal OperationFailureException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
          : base(key, createdStamp, message, scene, code, innerException, operatorCredential, data, hint)
        {
        }

        #endregion Constructor
    }
}
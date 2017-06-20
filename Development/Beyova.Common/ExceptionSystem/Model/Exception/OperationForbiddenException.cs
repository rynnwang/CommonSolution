using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

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
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        public OperationForbiddenException(string actionName, string reason, Exception innerException = null, object data = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base(string.Format("Operation [{0}] is forbidden. Reason: {1}", actionName, reason), new ExceptionCode { Major = ExceptionCode.MajorCode.OperationForbidden, Minor = reason }, innerException, data, hint, scene)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationForbiddenException"/> class.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="actionName">Name of the action.</param>
        public OperationForbiddenException(string reason, Exception innerException = null, object data = null, FriendlyHint hint = null, [CallerMemberName] string actionName = null)
           : base(string.Format("Operation [{0}] is forbidden. Reason: {1}", actionName, reason), new ExceptionCode { Major = ExceptionCode.MajorCode.OperationForbidden, Minor = reason }, innerException, data, hint, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationForbiddenException" /> class.
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
        internal OperationForbiddenException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
          : base(key, createdStamp, message, scene, code, innerException, operatorCredential, data, hint)
        {
        }

        #endregion Constructor
    }
}
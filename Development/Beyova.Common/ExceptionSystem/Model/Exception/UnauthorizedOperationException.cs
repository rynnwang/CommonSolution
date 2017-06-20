using System;
using System.Runtime.CompilerServices;
using Beyova.Api;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
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
        /// <param name="innerException">The inner exception.</param>
        /// <param name="reason">The minor code.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        public UnauthorizedOperationException(Exception innerException = null, string reason = null, object data = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base(string.Format("Unauthorized action on [{0}].", scene?.MethodName),
                  new ExceptionCode { Major = ExceptionCode.MajorCode.UnauthorizedOperation, Minor = reason.SafeToString("Operation") }, innerException, data, hint, scene)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedOperationException" /> class.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="operationName">Name of the operation.</param>
        public UnauthorizedOperationException(string reason = null, object data = null, FriendlyHint hint = null, [CallerMemberName] string operationName = null)
                 : base(string.Format("Unauthorized action on [{0}].", operationName),
                       new ExceptionCode { Major = ExceptionCode.MajorCode.UnauthorizedOperation, Minor = reason.SafeToString("Operation") }, data: data, hint: hint)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedOperationException" /> class.
        /// </summary>
        /// <param name="destinationMethod">The destination method.</param>
        /// <param name="permissionIdentifier">The permission identifier.</param>
        /// <param name="permission">The permission.</param>
        /// <param name="scene">The scene.</param>
        internal UnauthorizedOperationException(string destinationMethod, string permissionIdentifier, ApiPermission permission, ExceptionScene scene = null)
                 : base(string.Format(permission == ApiPermission.Required ? "Access denied to {0} by requirement of permission identifier: {1}" : "Access denied to {0} by owning permission identifier: {1}", destinationMethod, permissionIdentifier),
                       new ExceptionCode { Major = ExceptionCode.MajorCode.UnauthorizedOperation, Minor = ("PermissionIssue") }, scene: scene)
        {
            this.Hint = new FriendlyHint
            {
                CauseObjects = permissionIdentifier.AsArray(),
                Message = this.Message,
                HintCode = "401.Permission"
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedOperationException" /> class.
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
        internal UnauthorizedOperationException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
                  : base(key, createdStamp, message, scene, code, innerException, operatorCredential, data, hint)
        {
        }

        #endregion Constructor
    }
}
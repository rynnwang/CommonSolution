using System;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
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
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        public InvalidObjectException(string objectIdentity, Exception innerException = null, object data = null, string reason = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base(string.Format("Object [{0}] is invalid.", objectIdentity), new ExceptionCode { Major = ExceptionCode.MajorCode.NullOrInvalidValue, Minor = reason.SafeToString("InvalidObject") }, innerException, data, hint, scene)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidObjectException" /> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        internal InvalidObjectException(Exception innerException, object data = null, string reason = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base((innerException?.Message).SafeToString("Invalid object."), new ExceptionCode { Major = ExceptionCode.MajorCode.NullOrInvalidValue, Minor = reason.SafeToString("InvalidObject") }, innerException, data, hint, scene)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidObjectException" /> class.
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
        internal InvalidObjectException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
          : base(key, createdStamp, message, scene, code, innerException, operatorCredential, data, hint)
        {
        }

        #endregion
    }
}

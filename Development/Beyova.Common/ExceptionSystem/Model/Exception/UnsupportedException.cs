using System;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class UnsupportedException.
    /// </summary>
    public class UnsupportedException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedException" /> class.
        /// </summary>
        /// <param name="objectIdentifier">The object identifier.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        public UnsupportedException(string objectIdentifier, Exception innerException = null, string reason = null, object data = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base(string.Format("[{0}] is not supported .", objectIdentifier), new ExceptionCode { Major = ExceptionCode.MajorCode.CreditNotAfford, Minor = reason }, innerException, data, hint, scene)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException" /> class. This is used internally for restoring exception instance from ExceptionInfo.
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
        internal UnsupportedException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
          : base(key, createdStamp, message, scene, code, innerException, operatorCredential, data, hint)
        {
        }

        #endregion
    }
}

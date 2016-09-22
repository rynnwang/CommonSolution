using System;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
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
        /// <param name="target">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        public InitializationFailureException(string target, Exception innerException = null, string minor = null, object data = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base(string.Format("Failed to initialize [{0}].", target), new ExceptionCode { Major = ExceptionCode.MajorCode.ServiceUnavailable, Minor = minor.SafeToString("Initialize") }, innerException, data, hint, scene)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializationFailureException" /> class.
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
        internal InitializationFailureException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
          : base(key, createdStamp, message, scene, code, innerException, operatorCredential, data, hint)
        {
        }

        #endregion
    }
}

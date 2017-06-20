using System;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
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
        /// <param name="innerException">The inner exception.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        public CreditNotAffordException(string resourceName, string resourceIdentifier, Exception innerException = null, string minor = null, object data = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base(string.Format("Credit is not afford for resource [{0}] at [{1}].", resourceName, resourceIdentifier), new ExceptionCode { Major = ExceptionCode.MajorCode.CreditNotAfford, Minor = minor.SafeToString(resourceName) }, innerException, data, hint, scene)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditNotAffordException" /> class.
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
        internal CreditNotAffordException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
          : base(key, createdStamp, message, scene, code, innerException, operatorCredential, data, hint)
        {
        }

        #endregion Constructor
    }
}
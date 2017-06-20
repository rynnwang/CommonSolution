using System;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class ServiceUnavailableException.
    /// </summary>
    public class ServiceUnavailableException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceUnavailableException"/> class.
        /// </summary>
        /// <param name="serviceId">The service identifier.</param>
        /// <param name="minorReason">The minor reason.</param>
        public ServiceUnavailableException(string serviceId, string minorReason = null)
            : base(string.Format("Service [{0}] is unavailable due to {[1}].", serviceId, minorReason), new ExceptionCode { Major = ExceptionCode.MajorCode.ServiceUnavailable, Minor = minorReason.SafeToString("ServiceUnavailable") })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceUnavailableException" /> class.
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
        internal ServiceUnavailableException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
          : base(key, createdStamp, message, scene, code, innerException, operatorCredential, data, hint)
        {
        }

        #endregion Constructor
    }
}
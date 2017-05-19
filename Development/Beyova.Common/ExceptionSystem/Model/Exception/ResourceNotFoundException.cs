using System;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class ResourceNotFoundException.
    /// </summary>
    public class ResourceNotFoundException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException" /> class.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceIdentifier">The resource identifier.</param>
        /// <param name="scene">The scene.</param>
        public ResourceNotFoundException(string resourceName, string resourceIdentifier, ExceptionScene scene = null)
            : base(string.Format("Resource [{0}] of [{1}] is not found.", resourceIdentifier, resourceName), new ExceptionCode { Major = ExceptionCode.MajorCode.ResourceNotFound, Minor = resourceName }, scene: scene)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException" /> class.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        public ResourceNotFoundException(string resourceName)
            : base(string.Format("Resource [{0}] is not found.", resourceName), new ExceptionCode { Major = ExceptionCode.MajorCode.ResourceNotFound })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException" /> class.
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
        internal ResourceNotFoundException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
          : base(key, createdStamp, message, scene, code, innerException, operatorCredential, data, hint)
        {
        }

        #endregion
    }
}

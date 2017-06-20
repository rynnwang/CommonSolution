using System;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Exception class for data conflicts
    /// </summary>
    public class DataConflictException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConflictException" /> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        public DataConflictException(string entityName, string objectIdentity = null, Exception innerException = null, object data = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base(string.Format("Data conflicts for [{0}] at [{1}]", entityName, objectIdentity), new ExceptionCode { Major = ExceptionCode.MajorCode.DataConflict, Minor = entityName }, innerException, data, hint, scene)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConflictException" /> class.
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
        internal DataConflictException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
          : base(key, createdStamp, message, scene, code, innerException, operatorCredential, data, hint)
        {
        }

        #endregion Constructor
    }
}
using System;

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
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        public ResourceNotFoundException(string entityName, string objectIdentity, Exception innerException = null, string operatorIdentifier = null)
            : base(string.Format("Resource for [{0}] at [{1}] is not found.", entityName, objectIdentity), ExceptionCode.MajorCode.ResourceNotFound, null, innerException, operatorIdentifier)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException" /> class.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        public ResourceNotFoundException(string resourceName, string reason, string operatorIdentifier = null)
            : base(string.Format("Resource [{0}] is not found caused by [{1}].", resourceName, reason), ExceptionCode.MajorCode.ResourceNotFound, null, null, operatorIdentifier)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException"/> class.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        public ResourceNotFoundException(string resourceName, string operatorIdentifier = null)
            : base(string.Format("Resource [{0}] is not found.", resourceName), ExceptionCode.MajorCode.ResourceNotFound, null, null, operatorIdentifier)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        internal ResourceNotFoundException(string message, string operatorIdentifier, string minor, Exception innerException, object data)
            : base(message, new ExceptionCode { Major = ExceptionCode.MajorCode.ResourceNotFound, Minor = minor }, operatorIdentifier: operatorIdentifier, parameterData: data)
        {
        }

        #endregion
    }
}

using System;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class InvalidLicenseException.
    /// </summary>
    public class InvalidLicenseException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLicenseException" /> class.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        public InvalidLicenseException(string assemblyName)
            : base(string.Format("One or more required licenses are missing, invalid or expired for {0}.", assemblyName), new ExceptionCode { Major = ExceptionCode.MajorCode.ServiceUnavailable, Minor = "InvalidLicense" })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLicenseException" /> class.
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
        internal InvalidLicenseException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
          : base(key, createdStamp, message, scene, code, innerException, operatorCredential, data, hint)
        {
        }

        #endregion
    }
}

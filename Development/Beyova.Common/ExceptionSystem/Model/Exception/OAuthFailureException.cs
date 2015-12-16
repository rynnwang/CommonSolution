using System;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class OAuthFailureException.
    /// </summary>
    public class OAuthFailureException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthFailureException" /> class.
        /// </summary>
        /// <param name="oauthIdentity">The oauth identity.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        public OAuthFailureException(string oauthIdentity, string objectIdentity, Exception innerException = null, object data = null, string hintMessage = null)
            : base(string.Format("Failed to OAUTH for [{1}] in [{0}]", oauthIdentity, objectIdentity), new ExceptionCode { Major = ExceptionCode.MajorCode.UnauthorizedOperation, Minor = "OAuth" }, innerException, null, data, hintMessage: hintMessage)
        {
        }

        #endregion
    }
}

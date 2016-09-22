namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class UnauthorizedTokenException.
    /// </summary>
    public class UnauthorizedTokenException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedTokenException" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        public UnauthorizedTokenException(object data = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base("Unauthorized token or token expired.", new ExceptionCode { Major = ExceptionCode.MajorCode.UnauthorizedOperation, Minor = "Token" }, null, data, hint, scene)
        {
        }

        #endregion
    }
}

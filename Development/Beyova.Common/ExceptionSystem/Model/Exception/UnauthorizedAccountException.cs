namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class UnauthorizedAccountException.
    /// </summary>
    public class UnauthorizedAccountException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedOperationException" /> class.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        public UnauthorizedAccountException(string reason, object data = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base(string.Format("Failed to authenticate account caused by [{0}].", reason), new ExceptionCode { Major = ExceptionCode.MajorCode.UnauthorizedOperation, Minor = reason.SafeToString("Account") }, data: data, hint: hint, scene: scene)
        {
        }

        #endregion Constructor
    }
}
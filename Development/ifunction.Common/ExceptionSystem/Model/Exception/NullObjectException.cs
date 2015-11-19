
namespace ifunction.ExceptionSystem
{
    /// <summary>
    /// Class NullObjectException.
    /// </summary>
    public class NullObjectException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NullObjectException" /> class.
        /// </summary>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        public NullObjectException(string objectIdentity, object data = null, string hintMessage = null)
            : base(string.Format("[{0}] is null.", objectIdentity), new ExceptionCode { Major = ExceptionCode.MajorCode.NullOrInvalidValue, Minor = "NullObject" }, parameterData: data, hintMessage: hintMessage)
        {
        }

        #endregion
    }
}

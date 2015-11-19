
using System;

namespace ifunction.ExceptionSystem
{
    /// <summary>
    /// Class UnimplementedException.
    /// </summary>
    public class UnimplementedException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnimplementedException" /> class.
        /// </summary>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        public UnimplementedException(string objectIdentity, Exception innerException = null)
            : base(string.Format("[{0}] is not implemented.", objectIdentity), new ExceptionCode { Major = ExceptionCode.MajorCode.NotImplemented }, innerException)
        {
        }

        #endregion
    }
}

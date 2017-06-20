using System;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class InvalidExpressiontException.
    /// </summary>
    public class InvalidExpressiontException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidExpressiontException"/> class.
        /// </summary>
        /// <param name="expectObject">The expect object.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="position">The position.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        public InvalidExpressiontException(string expectObject, Exception innerException = null, string data = null, int? position = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base(string.Format("Expecting [{0}] when cast expression at position [{1}].", expectObject, position), new ExceptionCode { Major = ExceptionCode.MajorCode.NullOrInvalidValue, Minor = "InvalidExpression" }, innerException, data, hint, scene)
        {
        }

        #endregion Constructor
    }
}
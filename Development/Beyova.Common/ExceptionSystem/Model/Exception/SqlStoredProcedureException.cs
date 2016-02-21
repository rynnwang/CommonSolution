using System;
using System.Net;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class SqlStoredProcedureException.
    /// </summary>
    public class SqlStoredProcedureException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlStoredProcedureException"/> class.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="message">The message.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="reason">The reason.</param>
        public SqlStoredProcedureException(string storedProcedureName, string message, int errorCode, string reason)
            : base(string.Format("Error occurred when executing SQL stored procedure [{0}]. {1}", storedProcedureName, message),
                  new ExceptionCode { Major = (ExceptionCode.MajorCode)errorCode, Minor = reason })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlStoredProcedureException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exceptionCode">The exception code.</param>
        internal SqlStoredProcedureException(string message, ExceptionCode exceptionCode)
            : base(message, exceptionCode)
        {
        }

        #endregion
    }
}

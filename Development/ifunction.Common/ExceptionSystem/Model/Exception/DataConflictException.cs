using System;

namespace ifunction.ExceptionSystem
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
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        public DataConflictException(string entityName, string objectIdentity, Exception innerException = null, string operatorIdentifier = null, object data = null, string hintMessage = null)
            : base(string.Format("Data conflicts for [{0}] at [{1}]", entityName, objectIdentity), new ExceptionCode { Major = ExceptionCode.MajorCode.DataConflict, Minor = entityName }, innerException, operatorIdentifier, data, hintMessage: hintMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConflictException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hintMessage">The hint message.</param>
        internal DataConflictException(string message, string operatorIdentifier, string minor, Exception innerException, object data, string hintMessage = null)
            : base(message, new ExceptionCode { Major = ExceptionCode.MajorCode.DataConflict, Minor = minor }, operatorIdentifier: operatorIdentifier, parameterData: data, hintMessage: hintMessage)
        {
        }

        #endregion
    }
}

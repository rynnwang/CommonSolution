using System;
using System.Collections.Generic;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class BaseSystemException.
    /// </summary>
    public abstract class BaseException : System.Exception
    {
        #region Constants

        /// <summary>
        /// The data key_ reference data
        /// </summary>
        protected const string dataKey_ReferenceData = "Reference";

        /// <summary>
        /// The data key_ operator
        /// </summary>
        protected const string dataKey_Operator = "Operator";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the hint.
        /// </summary>
        /// <value>The hint.</value>
        public FriendlyHint Hint { get; protected set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public ExceptionCode Code
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid Key
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the reference data.
        /// </summary>
        /// <value>The reference data.</value>
        public object ParameterData
        {
            get
            {
                return this.DataReference[dataKey_ReferenceData];
            }
        }

        /// <summary>
        /// Gets the operator identifier.
        /// </summary>
        /// <value>The operator identifier.</value>
        public string OperatorIdentifier
        {
            get
            {
                return this.DataReference[dataKey_Operator].ObjectToString();
            }
        }

        /// <summary>
        /// Gets or sets the root cause.
        /// </summary>
        /// <value>The root cause.</value>
        public BaseException RootCause
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        public DateTime CreatedStamp { get; set; }

        /// <summary>
        /// Gets a collection of key/value pairs that provide additional user-defined information about the exception.
        /// </summary>
        /// <value>The data.</value>
        /// <returns>An object that implements the <see cref="T:System.Collections.IDictionary" /> interface and contains a collection of user-defined key/value pairs. The default is an empty collection.</returns>
        public Dictionary<string, object> DataReference { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException" /> class.
        /// </summary>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <param name="major">The major.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="parameterData">The parameter data.</param>
        /// <param name="hintMessage">The hint message.</param>
        protected BaseException(
          string exceptionMessage,
          ExceptionCode.MajorCode major,
          string minor = null,
          Exception innerException = null,
          string operatorIdentifier = null,
          object parameterData = null,
          string hintMessage = null)
            : this(exceptionMessage,
                  new ExceptionCode(major, minor), innerException,
                  operatorIdentifier,
                  parameterData,
                  string.IsNullOrWhiteSpace(hintMessage) ? null : new FriendlyHint { Cause = exceptionMessage, Code = new ExceptionCode(major, minor), Message = hintMessage })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <param name="exceptionCode">The exception code.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="parameterData">The parameter data.</param>
        /// <param name="hintMessage">The hint message.</param>
        protected BaseException(
            string exceptionMessage,
            ExceptionCode exceptionCode,
            Exception innerException = null,
            string operatorIdentifier = null,
            object parameterData = null,
            string hintMessage = null)
       : this(exceptionMessage,
             exceptionCode, innerException,
             operatorIdentifier,
             parameterData,
             string.IsNullOrWhiteSpace(hintMessage) ? null : new FriendlyHint { Cause = exceptionMessage, Code = exceptionCode, Message = hintMessage })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException" /> class.
        /// </summary>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <param name="exceptionCode">The exception code.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="parameterData">The parameter data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="overrideAsRootCause">if set to <c>true</c> [override as root cause].</param>
        protected BaseException(
            string exceptionMessage,
            ExceptionCode exceptionCode,
            Exception innerException,
            string operatorIdentifier,
            object parameterData,
            FriendlyHint hint,
            bool overrideAsRootCause = false)
            : base(exceptionMessage, innerException)
        {
            this.Code = exceptionCode;

            this.DataReference = new Dictionary<string, object>
            {
                {dataKey_Operator, operatorIdentifier},
                {dataKey_ReferenceData, parameterData}
            };

            this.CreatedStamp = DateTime.UtcNow;

            var inner = innerException as BaseException;
            this.Key = inner == null ? Guid.NewGuid() : inner.Key;
            this.RootCause = (overrideAsRootCause || inner == null) ? this : inner.RootCause;

            this.Hint = hint ?? inner?.Hint;
        }

        #endregion
    }
}



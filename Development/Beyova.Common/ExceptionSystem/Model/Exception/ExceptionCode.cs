using System.Diagnostics;
using System.Net;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Struct ExceptionCode
    /// </summary>
    public class ExceptionCode
    {
        /// <summary>
        /// Enum MajorCode
        /// </summary>
        public enum MajorCode
        {
            /// <summary>
            /// Value indicating it is undefined
            /// </summary>
            Undefined = 0,

            /// <summary>
            /// Value indicating it is null or invalid value
            /// </summary>
            NullOrInvalidValue = 400,

            /// <summary>
            /// Value indicating it is unauthorized operation
            /// </summary>
            UnauthorizedOperation = 401,

            /// <summary>
            /// Value indicating it is credit not afford
            /// </summary>
            CreditNotAfford = 402,

            /// <summary>
            /// Value indicating it is operation forbidden
            /// </summary>
            OperationForbidden = 403,

            /// <summary>
            /// Value indicating it is resource not found
            /// </summary>
            ResourceNotFound = 404,

            /// <summary>
            /// Value indicating it is data conflict
            /// </summary>
            DataConflict = 409,

            /// <summary>
            /// Value indicating it is operation failure
            /// </summary>
            OperationFailure = 500,

            /// <summary>
            /// Value indicating it is not implemented
            /// </summary>
            NotImplemented = 501,

            /// <summary>
            /// Value indicating it is service unavailable
            /// </summary>
            ServiceUnavailable = 503,

            /// <summary>
            /// Value indicating it is HTTP block error
            /// </summary>
            HttpBlockError = 504,

            /// <summary>
            /// Value indicating it is Unsupported
            /// </summary>
            Unsupported = 505
        }

        /// <summary>
        /// Gets or sets the major.
        /// </summary>
        /// <value>The major.</value>
        public MajorCode Major
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minor.
        /// <remarks>
        /// This value is to indicate the specific case for the major code.
        /// For instance, when <c>Major</c> is set as <c>UnauthorizedOperation</c>, this value can be set as <c>Token</c>, <c>User</c>, <c>Password</c> or <c>Expired</c> for describing case in simple.
        /// It is strongly recommended to set one word/term only.
        /// </remarks>
        /// </summary>
        /// <value>The minor.</value>
        public string Minor
        {
            get;
            set;
        }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCode"/> class.
        /// </summary>
        public ExceptionCode()
            : this(MajorCode.Undefined, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCode"/> struct.
        /// </summary>
        /// <param name="major">The major.</param>
        /// <param name="minor">The minor.</param>
        public ExceptionCode(MajorCode major, string minor = null)
        {
            this.Major = major;
            this.Minor = minor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCode"/> struct.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        public ExceptionCode(string stringValue)
        {
            string[] parts = stringValue.SafeToString().Split(new char[] { '.' }, 2);

            this.Major = parts.Length > 0 ? (MajorCode)(parts[0].ToInt32()) : MajorCode.Undefined;
            this.Minor = parts.Length > 1 ? parts[1] : null;
        }

        #endregion Constructor

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}.{1}", (int)this.Major, this.Minor).Trim('.');
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            ExceptionCode exceptionCode = obj as ExceptionCode;
            return exceptionCode != null && exceptionCode.Major == this.Major && exceptionCode.Minor.SafeToString() == this.Minor.SafeToString();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// To the type of the event log entry.
        /// </summary>
        /// <returns>EventLogEntryType.</returns>
        public EventLogEntryType ToEventLogEntryType()
        {
            EventLogEntryType result = EventLogEntryType.Information;

            switch (this.Major)
            {
                case MajorCode.OperationForbidden:
                case MajorCode.NullOrInvalidValue:
                case MajorCode.ServiceUnavailable:
                case MajorCode.OperationFailure:
                case MajorCode.NotImplemented:
                    result = EventLogEntryType.Error;
                    break;

                case MajorCode.DataConflict:
                case MajorCode.CreditNotAfford:
                case MajorCode.ResourceNotFound:
                    result = EventLogEntryType.Warning;
                    break;

                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// To the HTTP status code.
        /// </summary>
        /// <returns>HttpStatusCode.</returns>
        public HttpStatusCode ToHttpStatusCode()
        {
            return (HttpStatusCode)(int)(this.Major);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Int32"/> to <see cref="ExceptionCode"/>.
        /// </summary>
        /// <param name="exceptionMajorCode">The exception major code.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator ExceptionCode(int exceptionMajorCode)
        {
            return new ExceptionCode
            {
                Major = (MajorCode)exceptionMajorCode
            };
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="ExceptionCode"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="exceptionCode">The exception code.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator int(ExceptionCode exceptionCode)
        {
            return (int)exceptionCode.Major;
        }
    }
}
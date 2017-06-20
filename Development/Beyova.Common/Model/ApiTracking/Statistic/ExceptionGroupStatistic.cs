using Beyova.ExceptionSystem;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ExceptionGroupStatistic.
    /// </summary>
    public class ExceptionGroupStatistic : GroupStatistic
    {
        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>The service identifier.</value>
        public string ServiceIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the server identifier.
        /// </summary>
        /// <value>The server identifier.</value>
        public string ServerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the exception code.
        /// </summary>
        /// <value>The exception code.</value>
        public ExceptionCode.MajorCode ExceptionCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionGroupStatistic" /> class.
        /// </summary>
        public ExceptionGroupStatistic() : base()
        {
        }
    }
}
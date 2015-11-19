using ifunction.ApiTracking.Model;
using Newtonsoft.Json.Linq;

namespace ifunction.ExceptionSystem
{
    /// <summary>
    /// Class ExceptionBase.
    /// </summary>
    public class ExceptionBase : ApiLogBase, IExceptionInfo
    {
        #region Property

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the target site.
        /// </summary>
        /// <value>The target site.</value>
        public string TargetSite { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        /// <value>The stack trace.</value>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public ExceptionCode Code { get; set; }

        /// <summary>
        /// Gets or sets the type of the exception.
        /// </summary>
        /// <value>The type of the exception.</value>
        public string ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        public ExceptionInfo.ExceptionCriticality? Level { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public JObject Data { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBase" /> class.
        /// </summary>
        /// <param name="exceptionBase">The exception base.</param>
        public ExceptionBase(ExceptionBase exceptionBase = null)
            : base(exceptionBase)
        {
            if (exceptionBase != null)
            {
                this.Message = exceptionBase.Message;
                this.TargetSite = exceptionBase.TargetSite;
                this.StackTrace = exceptionBase.StackTrace;
                this.Code = exceptionBase.Code;
                this.Level = exceptionBase.Level;
                this.Source = exceptionBase.Source;
                this.Data = exceptionBase.Data;
            }
        }
    }
}

﻿using Beyova.ApiTracking;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class ExceptionBase.
    /// </summary>
    public class ExceptionBase : ApiLogBase
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
        /// Gets or sets the operator credential.
        /// </summary>
        /// <value>The operator credential.</value>
        public BaseCredential OperatorCredential { get; set; }

        #endregion Property

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
                this.Level = exceptionBase.Level;
                this.Source = exceptionBase.Source;
                this.OperatorCredential = exceptionBase.OperatorCredential;
            }
        }
    }
}
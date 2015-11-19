using System;
using System.Runtime.Serialization;

namespace ifunction.ExceptionSystem
{
    /// <summary>
    /// Class FriendlyHint.
    /// </summary>
    public class FriendlyHint
    {
        /// <summary>
        /// Gets or sets the cause.
        /// </summary>
        /// <value>The cause.</value>
        public string Cause { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public ExceptionCode Code { get; set; }
    }
}

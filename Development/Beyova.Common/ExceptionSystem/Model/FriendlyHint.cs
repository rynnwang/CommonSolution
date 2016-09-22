using System;
using System.Runtime.Serialization;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class FriendlyHint.
    /// </summary>
    public class FriendlyHint
    {
        /// <summary>
        /// Gets or sets the cause objects.
        /// </summary>
        /// <value>The cause objects.</value>
        public string[] CauseObjects { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the hint code.
        /// </summary>
        /// <value>The hint code.</value>
        public string HintCode { get; set; }
    }
}
